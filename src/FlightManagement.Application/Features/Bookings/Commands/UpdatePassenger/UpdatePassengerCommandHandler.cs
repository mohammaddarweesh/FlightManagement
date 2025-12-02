using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FlightManagement.Application.Features.Bookings.Commands.UpdatePassenger;

/// <summary>
/// Handler for updating passenger information.
/// </summary>
public class UpdatePassengerCommandHandler : ICommandHandler<UpdatePassengerCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePassengerCommandHandler> _logger;

    public UpdatePassengerCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdatePassengerCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdatePassengerCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating passenger: {PassengerId} in booking: {BookingId}", 
            request.PassengerId, request.BookingId);

        // Validate booking
        var bookingRepo = _unitOfWork.Repository<Booking>();
        var booking = await bookingRepo.GetByIdAsync(request.BookingId, cancellationToken);
        if (booking == null)
            return Result.Failure($"Booking with ID '{request.BookingId}' not found");

        if (booking.Status == BookingStatus.Cancelled || booking.Status == BookingStatus.Completed)
            return Result.Failure($"Cannot modify booking with status: {booking.Status}");

        // Get passenger
        var passengerRepo = _unitOfWork.Repository<Passenger>();
        var passenger = await passengerRepo.FirstOrDefaultAsync(
            p => p.Id == request.PassengerId && p.BookingId == request.BookingId,
            cancellationToken);

        if (passenger == null)
            return Result.Failure("Passenger not found in this booking");

        // Track changes
        var oldValues = new Dictionary<string, object?>();
        var newValues = new Dictionary<string, object?>();

        UpdateField(request.Title, passenger.Title, v => passenger.Title = v!, "Title", oldValues, newValues);
        UpdateField(request.FirstName, passenger.FirstName, v => passenger.FirstName = v!, "FirstName", oldValues, newValues);
        UpdateField(request.MiddleName, passenger.MiddleName, v => passenger.MiddleName = v, "MiddleName", oldValues, newValues);
        UpdateField(request.LastName, passenger.LastName, v => passenger.LastName = v!, "LastName", oldValues, newValues);
        UpdateField(request.PassportNumber, passenger.PassportNumber, v => passenger.PassportNumber = v, "PassportNumber", oldValues, newValues);
        UpdateField(request.PassportIssuingCountry, passenger.PassportIssuingCountry, v => passenger.PassportIssuingCountry = v, "PassportIssuingCountry", oldValues, newValues);
        UpdateField(request.Email, passenger.Email, v => passenger.Email = v, "Email", oldValues, newValues);
        UpdateField(request.Phone, passenger.Phone, v => passenger.Phone = v, "Phone", oldValues, newValues);
        UpdateField(request.SpecialAssistance, passenger.SpecialAssistance, v => passenger.SpecialAssistance = v, "SpecialAssistance", oldValues, newValues);
        UpdateField(request.FrequentFlyerNumber, passenger.FrequentFlyerNumber, v => passenger.FrequentFlyerNumber = v, "FrequentFlyerNumber", oldValues, newValues);

        if (request.PassportExpiryDate.HasValue && request.PassportExpiryDate != passenger.PassportExpiryDate)
        {
            oldValues["PassportExpiryDate"] = passenger.PassportExpiryDate;
            newValues["PassportExpiryDate"] = request.PassportExpiryDate;
            passenger.PassportExpiryDate = request.PassportExpiryDate;
        }

        if (request.MealPreference.HasValue && request.MealPreference != passenger.MealPreference)
        {
            oldValues["MealPreference"] = passenger.MealPreference;
            newValues["MealPreference"] = request.MealPreference;
            passenger.MealPreference = request.MealPreference;
        }

        if (request.FrequentFlyerAirlineId.HasValue && request.FrequentFlyerAirlineId != passenger.FrequentFlyerAirlineId)
        {
            oldValues["FrequentFlyerAirlineId"] = passenger.FrequentFlyerAirlineId;
            newValues["FrequentFlyerAirlineId"] = request.FrequentFlyerAirlineId;
            passenger.FrequentFlyerAirlineId = request.FrequentFlyerAirlineId;
        }

        if (oldValues.Count == 0)
            return Result.Success(); // No changes

        passengerRepo.Update(passenger);

        // Add history entry
        var historyRepo = _unitOfWork.Repository<BookingHistory>();
        var history = new BookingHistory
        {
            Id = Guid.NewGuid(),
            BookingId = booking.Id,
            Action = BookingAction.PassengerUpdated,
            Description = $"Passenger {passenger.FirstName} {passenger.LastName} information updated",
            OldValues = JsonSerializer.Serialize(oldValues),
            NewValues = JsonSerializer.Serialize(newValues),
            PerformedByType = ActorType.Customer,
            PerformedAt = DateTime.UtcNow
        };
        await historyRepo.AddAsync(history, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Passenger updated: {PassengerId}", request.PassengerId);
        return Result.Success();
    }

    private static void UpdateField(string? newValue, string? oldValue, Action<string?> setter, 
        string fieldName, Dictionary<string, object?> oldValues, Dictionary<string, object?> newValues)
    {
        if (!string.IsNullOrEmpty(newValue) && newValue != oldValue)
        {
            oldValues[fieldName] = oldValue;
            newValues[fieldName] = newValue;
            setter(newValue);
        }
    }
}

