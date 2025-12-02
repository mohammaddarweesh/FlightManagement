using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FlightManagement.Application.Features.Bookings.Commands.UpdateBooking;

/// <summary>
/// Handler for updating booking contact information.
/// </summary>
public class UpdateBookingCommandHandler : ICommandHandler<UpdateBookingCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateBookingCommandHandler> _logger;

    public UpdateBookingCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateBookingCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateBookingCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating booking: {BookingId}", request.BookingId);

        var bookingRepo = _unitOfWork.Repository<Booking>();
        var booking = await bookingRepo.GetByIdAsync(request.BookingId, cancellationToken);

        if (booking == null)
            return Result.Failure($"Booking with ID '{request.BookingId}' not found");

        if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Completed)
            return Result.Failure($"Cannot modify booking with status: {booking.Status}");

        // Track changes for history
        var oldValues = new Dictionary<string, string?>();
        var newValues = new Dictionary<string, string?>();

        if (!string.IsNullOrEmpty(request.ContactEmail) && request.ContactEmail != booking.ContactEmail)
        {
            oldValues["ContactEmail"] = booking.ContactEmail;
            newValues["ContactEmail"] = request.ContactEmail;
            booking.ContactEmail = request.ContactEmail;
        }

        if (!string.IsNullOrEmpty(request.ContactPhone) && request.ContactPhone != booking.ContactPhone)
        {
            oldValues["ContactPhone"] = booking.ContactPhone;
            newValues["ContactPhone"] = request.ContactPhone;
            booking.ContactPhone = request.ContactPhone;
        }

        if (request.SpecialRequests != null && request.SpecialRequests != booking.SpecialRequests)
        {
            oldValues["SpecialRequests"] = booking.SpecialRequests;
            newValues["SpecialRequests"] = request.SpecialRequests;
            booking.SpecialRequests = request.SpecialRequests;
        }

        if (oldValues.Count == 0)
            return Result.Success(); // No changes

        bookingRepo.Update(booking);

        // Add history entry
        var historyRepo = _unitOfWork.Repository<BookingHistory>();
        var history = new BookingHistory
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            Action = BookingAction.ContactUpdated,
            Description = "Booking contact information updated",
            OldValues = JsonSerializer.Serialize(oldValues),
            NewValues = JsonSerializer.Serialize(newValues),
            PerformedByType = ActorType.Customer,
            PerformedAt = DateTime.UtcNow
        };
        await historyRepo.AddAsync(history, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Booking updated: {BookingReference}", booking.BookingReference);
        return Result.Success();
    }
}

