namespace FlightManagement.Domain.Enums;

/// <summary>
/// Types of booking policies.
/// </summary>
public enum PolicyType
{
    /// <summary>
    /// Minimum advance purchase requirement.
    /// </summary>
    MinimumAdvancePurchase = 0,

    /// <summary>
    /// Maximum advance booking limit.
    /// </summary>
    MaximumAdvanceBooking = 1,

    /// <summary>
    /// Minimum passengers required for booking.
    /// </summary>
    MinimumPassengers = 2,

    /// <summary>
    /// Maximum passengers allowed per booking.
    /// </summary>
    MaximumPassengers = 3,

    /// <summary>
    /// Age restriction policy.
    /// </summary>
    AgeRestriction = 4,

    /// <summary>
    /// Route restriction policy.
    /// </summary>
    RouteRestriction = 5
}

