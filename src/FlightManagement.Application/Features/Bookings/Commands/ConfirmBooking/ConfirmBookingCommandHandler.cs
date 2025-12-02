using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Bookings.Commands.ConfirmBooking;

/// <summary>
/// Handler for confirming a booking after payment.
/// </summary>
public class ConfirmBookingCommandHandler : ICommandHandler<ConfirmBookingCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ConfirmBookingCommandHandler> _logger;

    public ConfirmBookingCommandHandler(IUnitOfWork unitOfWork, ILogger<ConfirmBookingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(ConfirmBookingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Confirming booking: {BookingId}", request.BookingId);

        var bookingRepo = _unitOfWork.Repository<Booking>();
        var booking = await bookingRepo.GetByIdAsync(request.BookingId, cancellationToken);

        if (booking == null)
            return Result.Failure($"Booking with ID '{request.BookingId}' not found");

        if (booking.Status != BookingStatus.Pending)
            return Result.Failure($"Booking is not in pending status. Current status: {booking.Status}");

        if (booking.ExpiresAt.HasValue && booking.ExpiresAt < DateTime.UtcNow)
            return Result.Failure("Booking has expired");

        if (request.Amount < booking.TotalAmount)
            return Result.Failure($"Payment amount ({request.Amount}) is less than total amount ({booking.TotalAmount})");

        // Create payment record
        var paymentRepo = _unitOfWork.Repository<PaymentRecord>();
        var payment = new PaymentRecord
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            TransactionReference = request.TransactionReference,
            PaymentType = PaymentType.Initial,
            PaymentMethod = request.PaymentMethod,
            Amount = request.Amount,
            Currency = booking.Currency,
            Status = PaymentStatus.Completed,
            ProcessedAt = DateTime.UtcNow,
            CardLastFour = request.CardLastFour,
            CardBrand = request.CardBrand
        };
        await paymentRepo.AddAsync(payment, cancellationToken);

        // Update booking status
        booking.Status = BookingStatus.Confirmed;
        booking.PaymentStatus = PaymentStatus.Completed;
        booking.PaidAmount = request.Amount;
        booking.ExpiresAt = null; // No longer expires
        bookingRepo.Update(booking);

        // Add history entry
        var historyRepo = _unitOfWork.Repository<BookingHistory>();
        var history = new BookingHistory
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            Action = BookingAction.Confirmed,
            Description = $"Booking confirmed with payment of {request.Amount} {booking.Currency}",
            PerformedByType = ActorType.System,
            PerformedAt = DateTime.UtcNow
        };
        await historyRepo.AddAsync(history, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Booking confirmed: {BookingReference}", booking.BookingReference);
        return Result.Success();
    }
}

