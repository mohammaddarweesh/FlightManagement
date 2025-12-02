using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Common.Interfaces;

/// <summary>
/// Service for checking flight availability including overbooking policies.
/// </summary>
public interface IAvailabilityService
{
    /// <summary>
    /// Checks if a flight has availability for the requested seats considering overbooking policies.
    /// </summary>
    Task<AvailabilityCheckResult> CheckAvailabilityAsync(
        Guid flightId,
        FlightClass cabinClass,
        int requestedSeats,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the maximum bookable seats for a flight class including overbooking allowance.
    /// </summary>
    Task<int> GetMaxBookableSeatsAsync(
        Guid flightId,
        FlightClass cabinClass,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Validates booking policies for a flight.
    /// </summary>
    Task<PolicyValidationResult> ValidateBookingPoliciesAsync(
        Guid flightId,
        FlightClass cabinClass,
        DateTime bookingDate,
        int passengerCount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if a date falls within a blackout period.
    /// </summary>
    Task<BlackoutCheckResult> CheckBlackoutDatesAsync(
        Guid flightId,
        DateTime travelDate,
        bool checkPromotions = false,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of availability check.
/// </summary>
public record AvailabilityCheckResult(
    bool IsAvailable,
    int AvailableSeats,
    int MaxBookableSeats,
    int OverbookingAllowance,
    string? Message
);

/// <summary>
/// Result of policy validation.
/// </summary>
public record PolicyValidationResult(
    bool IsValid,
    List<PolicyViolation> Violations
);

/// <summary>
/// Details of a policy violation.
/// </summary>
public record PolicyViolation(
    string PolicyCode,
    string PolicyName,
    string ErrorMessage
);

/// <summary>
/// Result of blackout date check.
/// </summary>
public record BlackoutCheckResult(
    bool IsBlackout,
    bool BlocksBookings,
    bool BlocksPromotions,
    string? BlackoutName,
    string? Message
);

