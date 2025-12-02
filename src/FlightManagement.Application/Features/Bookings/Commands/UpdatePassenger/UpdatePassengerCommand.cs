using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Bookings.Commands.UpdatePassenger;

/// <summary>
/// Command to update passenger information.
/// </summary>
public record UpdatePassengerCommand(
    Guid BookingId,
    Guid PassengerId,
    string? Title = null,
    string? FirstName = null,
    string? MiddleName = null,
    string? LastName = null,
    string? PassportNumber = null,
    string? PassportIssuingCountry = null,
    DateTime? PassportExpiryDate = null,
    string? Email = null,
    string? Phone = null,
    MealPreference? MealPreference = null,
    string? SpecialAssistance = null,
    string? FrequentFlyerNumber = null,
    Guid? FrequentFlyerAirlineId = null
) : ICommand;

