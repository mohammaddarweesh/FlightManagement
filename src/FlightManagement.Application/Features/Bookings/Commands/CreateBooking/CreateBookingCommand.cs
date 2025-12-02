using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Bookings.Commands.CreateBooking;

/// <summary>
/// Command to create a new booking.
/// </summary>
public record CreateBookingCommand(
    Guid CustomerId,
    TripType TripType,
    string ContactEmail,
    string ContactPhone,
    string? SpecialRequests,
    Guid? CancellationPolicyId,
    string? PromoCode,
    List<BookingSegmentInput> Segments,
    List<PassengerInput> Passengers
) : ICommand<CreateBookingResult>;

/// <summary>
/// Input for a booking segment (flight leg).
/// </summary>
public record BookingSegmentInput(
    Guid FlightId,
    FlightClass CabinClass,
    int SegmentOrder
);

/// <summary>
/// Input for a passenger.
/// </summary>
public record PassengerInput(
    PassengerType PassengerType,
    string Title,
    string FirstName,
    string? MiddleName,
    string LastName,
    DateTime DateOfBirth,
    Gender Gender,
    string Nationality,
    string? PassportNumber,
    string? PassportIssuingCountry,
    DateTime? PassportExpiryDate,
    string? Email,
    string? Phone,
    MealPreference? MealPreference,
    string? SpecialAssistance,
    string? FrequentFlyerNumber,
    Guid? FrequentFlyerAirlineId,
    bool IsPrimaryContact,
    bool IsLeadPassenger
);

/// <summary>
/// Result of booking creation.
/// </summary>
public record CreateBookingResult(
    Guid BookingId,
    string BookingReference,
    decimal TotalAmount,
    string Currency,
    DateTime? ExpiresAt
);

