using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Bookings.Commands.AddExtra;

/// <summary>
/// Handler for adding an extra service to a booking.
/// </summary>
public class AddExtraCommandHandler : ICommandHandler<AddExtraCommand, AddExtraResult>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AddExtraCommandHandler> _logger;

    public AddExtraCommandHandler(IUnitOfWork unitOfWork, ILogger<AddExtraCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<AddExtraResult>> Handle(AddExtraCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding extra to booking: {BookingId}, Type: {ExtraType}", 
            request.BookingId, request.ExtraType);

        // Validate booking
        var bookingRepo = _unitOfWork.Repository<Booking>();
        var booking = await bookingRepo.GetByIdAsync(request.BookingId, cancellationToken);
        if (booking == null)
            return Result<AddExtraResult>.Failure($"Booking with ID '{request.BookingId}' not found");

        if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Completed)
            return Result<AddExtraResult>.Failure($"Cannot modify booking with status: {booking.Status}");

        // Validate segment if provided
        if (request.BookingSegmentId.HasValue)
        {
            var segmentRepo = _unitOfWork.Repository<BookingSegment>();
            var segment = await segmentRepo.FirstOrDefaultAsync(
                s => s.Id == request.BookingSegmentId && s.BookingId == request.BookingId,
                cancellationToken);
            if (segment == null)
                return Result<AddExtraResult>.Failure("Segment not found in this booking");
        }

        // Validate passenger if provided
        if (request.PassengerId.HasValue)
        {
            var passengerRepo = _unitOfWork.Repository<Passenger>();
            var passenger = await passengerRepo.FirstOrDefaultAsync(
                p => p.Id == request.PassengerId && p.BookingId == request.BookingId,
                cancellationToken);
            if (passenger == null)
                return Result<AddExtraResult>.Failure("Passenger not found in this booking");
        }

        // Validate flight amenity if provided
        if (request.FlightAmenityId.HasValue)
        {
            var amenityRepo = _unitOfWork.Repository<FlightAmenity>();
            var amenity = await amenityRepo.GetByIdAsync(request.FlightAmenityId.Value, cancellationToken);
            if (amenity == null)
                return Result<AddExtraResult>.Failure("Flight amenity not found");
        }

        // Calculate total price
        decimal totalPrice = request.UnitPrice * request.Quantity;

        // Create booking extra
        var extraRepo = _unitOfWork.Repository<BookingExtra>();
        var extra = new BookingExtra
        {
            Id = Guid.NewGuid(),
            BookingId = request.BookingId,
            BookingSegmentId = request.BookingSegmentId,
            PassengerId = request.PassengerId,
            FlightAmenityId = request.FlightAmenityId,
            ExtraType = request.ExtraType,
            Description = request.Description,
            Quantity = request.Quantity,
            UnitPrice = request.UnitPrice,
            TotalPrice = totalPrice,
            Currency = booking.Currency,
            Status = ExtraStatus.Confirmed
        };
        await extraRepo.AddAsync(extra, cancellationToken);

        // Update booking totals
        booking.ExtrasFees += totalPrice;
        booking.TotalAmount += totalPrice;
        bookingRepo.Update(booking);

        // Add history entry
        var historyRepo = _unitOfWork.Repository<BookingHistory>();
        var history = new BookingHistory
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            Action = BookingAction.ExtraAdded,
            Description = $"Added {request.Quantity}x {request.ExtraType}: {request.Description} ({totalPrice} {booking.Currency})",
            PerformedByType = ActorType.Customer,
            PerformedAt = DateTime.UtcNow
        };
        await historyRepo.AddAsync(history, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Extra added to booking: {BookingId}, Extra ID: {ExtraId}", 
            request.BookingId, extra.Id);

        return Result<AddExtraResult>.Success(new AddExtraResult(
            extra.Id,
            totalPrice,
            booking.TotalAmount
        ));
    }
}

