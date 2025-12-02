using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Bookings.Commands.CancelBooking;

/// <summary>
/// Handler for cancelling a booking.
/// </summary>
public class CancelBookingCommandHandler : ICommandHandler<CancelBookingCommand, CancelBookingResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CancelBookingCommandHandler> _logger;

    public CancelBookingCommandHandler(IUnitOfWork unitOfWork, ILogger<CancelBookingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<CancelBookingResult>> Handle(CancelBookingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling booking: {BookingId}", request.BookingId);

        var bookingRepo = _unitOfWork.Repository<Booking>();
        var booking = await bookingRepo.Query()
            .Include(b => b.Segments)
            .Include(b => b.CancellationPolicy)
                .ThenInclude(cp => cp!.Rules)
            .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

        if (booking == null)
            return Result<CancelBookingResult>.Failure($"Booking with ID '{request.BookingId}' not found");

        if (booking.Status == BookingStatus.Cancelled)
            return Result<CancelBookingResult>.Failure("Booking is already cancelled");

        if (booking.Status == BookingStatus.Completed)
            return Result<CancelBookingResult>.Failure("Cannot cancel a completed booking");

        // Calculate refund based on cancellation policy
        var (isRefundable, refundAmount, cancellationFee) = CalculateRefund(booking);

        // Update booking status
        booking.Status = BookingStatus.Cancelled;
        booking.CancelledAt = DateTime.UtcNow;
        booking.CancellationReason = request.CancellationReason;
        booking.RefundAmount = refundAmount;
        booking.RefundStatus = refundAmount > 0 ? RefundStatus.Pending : null;
        bookingRepo.Update(booking);

        // Update segment statuses
        var segmentRepo = _unitOfWork.Repository<BookingSegment>();
        foreach (var segment in booking.Segments)
        {
            segment.Status = SegmentStatus.Cancelled;
            segmentRepo.Update(segment);
        }

        // Release any reserved seats
        await ReleaseSeatReservations(booking.Id, cancellationToken);

        // Create refund payment record if applicable
        if (refundAmount > 0)
        {
            var paymentRepo = _unitOfWork.Repository<PaymentRecord>();
            var refundRecord = new PaymentRecord
            {
                Id = Guid.NewGuid(),
                BookingId = booking.Id,
                TransactionReference = $"REFUND-{booking.BookingReference}-{DateTime.UtcNow:yyyyMMddHHmmss}",
                PaymentType = PaymentType.Refund,
                PaymentMethod = PaymentMethod.BankTransfer, // Refund via bank transfer
                Amount = refundAmount,
                Currency = booking.Currency,
                Status = PaymentStatus.Pending
            };
            await paymentRepo.AddAsync(refundRecord, cancellationToken);
        }

        // Add history entry
        var historyRepo = _unitOfWork.Repository<BookingHistory>();
        var history = new BookingHistory
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            Action = BookingAction.Cancelled,
            Description = $"Booking cancelled. Refund: {refundAmount} {booking.Currency}. Reason: {request.CancellationReason ?? "Not specified"}",
            PerformedByType = ActorType.Customer,
            PerformedAt = DateTime.UtcNow
        };
        await historyRepo.AddAsync(history, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Booking cancelled: {BookingReference}, Refund: {RefundAmount}", 
            booking.BookingReference, refundAmount);

        string message = isRefundable
            ? $"Booking cancelled. A refund of {refundAmount} {booking.Currency} will be processed."
            : "Booking cancelled. No refund applicable based on cancellation policy.";

        return Result<CancelBookingResult>.Success(new CancelBookingResult(
            isRefundable,
            refundAmount,
            cancellationFee,
            message
        ));
    }

    private (bool IsRefundable, decimal RefundAmount, decimal CancellationFee) CalculateRefund(Booking booking)
    {
        if (booking.PaidAmount <= 0)
            return (false, 0, 0);

        if (booking.CancellationPolicy == null || !booking.CancellationPolicy.IsRefundable)
            return (false, 0, booking.PaidAmount);

        // Find earliest departure time
        var earliestDeparture = booking.Segments
            .Select(s => s.Flight?.ScheduledDepartureTime ?? DateTime.MaxValue)
            .Min();

        var hoursBeforeDeparture = (earliestDeparture - DateTime.UtcNow).TotalHours;

        // Find applicable rule
        var applicableRule = booking.CancellationPolicy.Rules
            .Where(r => hoursBeforeDeparture >= r.MinHoursBeforeDeparture)
            .OrderByDescending(r => r.MinHoursBeforeDeparture)
            .FirstOrDefault();

        if (applicableRule == null)
            return (false, 0, booking.PaidAmount);

        decimal refundPercentage = applicableRule.RefundPercentage;
        decimal flatFee = applicableRule.FlatFee;

        decimal refundAmount = (booking.PaidAmount * refundPercentage / 100) - flatFee;
        refundAmount = Math.Max(0, refundAmount);

        decimal cancellationFee = booking.PaidAmount - refundAmount;

        return (refundAmount > 0, refundAmount, cancellationFee);
    }

    private async Task ReleaseSeatReservations(Guid bookingId, CancellationToken cancellationToken)
    {
        var passengerSeatRepo = _unitOfWork.Repository<PassengerSeat>();
        var flightSeatRepo = _unitOfWork.Repository<FlightSeat>();

        var passengerSeats = await passengerSeatRepo.FindAsync(
            ps => ps.Passenger.BookingId == bookingId,
            cancellationToken);

        foreach (var ps in passengerSeats)
        {
            var flightSeat = await flightSeatRepo.GetByIdAsync(ps.FlightSeatId, cancellationToken);
            if (flightSeat != null && (flightSeat.Status == SeatStatus.Reserved || flightSeat.Status == SeatStatus.Booked))
            {
                flightSeat.Status = SeatStatus.Available;
                flightSeat.BookingId = null;
                flightSeatRepo.Update(flightSeat);
            }
        }
    }
}

