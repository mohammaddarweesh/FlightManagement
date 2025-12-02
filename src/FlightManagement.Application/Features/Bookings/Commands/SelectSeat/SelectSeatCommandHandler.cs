using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Bookings.Commands.SelectSeat;

/// <summary>
/// Handler for selecting a seat for a passenger.
/// </summary>
public class SelectSeatCommandHandler : ICommandHandler<SelectSeatCommand, SelectSeatResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<SelectSeatCommandHandler> _logger;

    public SelectSeatCommandHandler(IUnitOfWork unitOfWork, ILogger<SelectSeatCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<SelectSeatResult>> Handle(SelectSeatCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Selecting seat for passenger: {PassengerId} on segment: {SegmentId}", 
            request.PassengerId, request.BookingSegmentId);

        // Validate booking
        var bookingRepo = _unitOfWork.Repository<Booking>();
        var booking = await bookingRepo.GetByIdAsync(request.BookingId, cancellationToken);
        if (booking == null)
            return Result<SelectSeatResult>.Failure($"Booking with ID '{request.BookingId}' not found");

        if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Completed)
            return Result<SelectSeatResult>.Failure($"Cannot modify booking with status: {booking.Status}");

        // Validate passenger belongs to booking
        var passengerRepo = _unitOfWork.Repository<Passenger>();
        var passenger = await passengerRepo.FirstOrDefaultAsync(
            p => p.Id == request.PassengerId && p.BookingId == request.BookingId,
            cancellationToken);
        if (passenger == null)
            return Result<SelectSeatResult>.Failure("Passenger not found in this booking");

        // Validate segment belongs to booking
        var segmentRepo = _unitOfWork.Repository<BookingSegment>();
        var segment = await segmentRepo.FirstOrDefaultAsync(
            s => s.Id == request.BookingSegmentId && s.BookingId == request.BookingId,
            cancellationToken);
        if (segment == null)
            return Result<SelectSeatResult>.Failure("Segment not found in this booking");

        // Validate flight seat
        var flightSeatRepo = _unitOfWork.Repository<FlightSeat>();
        var flightSeat = await flightSeatRepo.Query()
            .Include(fs => fs.Seat)
            .FirstOrDefaultAsync(fs => fs.Id == request.FlightSeatId, cancellationToken);
        if (flightSeat == null)
            return Result<SelectSeatResult>.Failure("Flight seat not found");

        if (flightSeat.FlightId != segment.FlightId)
            return Result<SelectSeatResult>.Failure("Seat does not belong to the flight in this segment");

        if (flightSeat.Status != SeatStatus.Available)
            return Result<SelectSeatResult>.Failure($"Seat is not available. Current status: {flightSeat.Status}");

        // Check if passenger already has a seat on this segment
        var passengerSeatRepo = _unitOfWork.Repository<PassengerSeat>();
        var existingSeat = await passengerSeatRepo.FirstOrDefaultAsync(
            ps => ps.PassengerId == request.PassengerId && ps.BookingSegmentId == request.BookingSegmentId,
            cancellationToken);

        if (existingSeat != null)
        {
            // Release the old seat
            var oldFlightSeat = await flightSeatRepo.GetByIdAsync(existingSeat.FlightSeatId, cancellationToken);
            if (oldFlightSeat != null)
            {
                oldFlightSeat.Status = SeatStatus.Available;
                oldFlightSeat.BookingId = null;
                flightSeatRepo.Update(oldFlightSeat);
            }

            // Update booking total (remove old seat fee)
            booking.SeatSelectionFees -= existingSeat.SeatFee;
            booking.TotalAmount -= existingSeat.SeatFee;

            // Remove old passenger seat
            passengerSeatRepo.Delete(existingSeat);
        }

        // Reserve the new seat
        flightSeat.Status = SeatStatus.Reserved;
        flightSeat.BookingId = booking.Id;
        flightSeatRepo.Update(flightSeat);

        // Create passenger seat assignment
        var seatFee = flightSeat.Price ?? 0;
        var passengerSeat = new PassengerSeat
        {
            Id = Guid.NewGuid(),
            PassengerId = request.PassengerId,
            BookingSegmentId = request.BookingSegmentId,
            FlightSeatId = request.FlightSeatId,
            SeatNumber = flightSeat.Seat?.SeatNumber ?? "Unknown",
            SeatFee = seatFee,
            AssignmentType = SeatAssignmentType.Selected
        };
        await passengerSeatRepo.AddAsync(passengerSeat, cancellationToken);

        // Update booking totals
        booking.SeatSelectionFees += seatFee;
        booking.TotalAmount += seatFee;
        bookingRepo.Update(booking);

        // Add history entry
        var historyRepo = _unitOfWork.Repository<BookingHistory>();
        var history = new BookingHistory
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            Action = BookingAction.SeatSelected,
            Description = $"Seat {passengerSeat.SeatNumber} selected for {passenger.FirstName} {passenger.LastName}",
            PerformedByType = ActorType.Customer,
            PerformedAt = DateTime.UtcNow
        };
        await historyRepo.AddAsync(history, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seat selected: {SeatNumber} for passenger: {PassengerId}", 
            passengerSeat.SeatNumber, request.PassengerId);

        return Result<SelectSeatResult>.Success(new SelectSeatResult(
            passengerSeat.Id,
            passengerSeat.SeatNumber,
            seatFee
        ));
    }
}

