using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Defines booking policies such as minimum advance purchase requirements.
/// </summary>
public class BookingPolicy : BaseEntity
{
    /// <summary>
    /// Unique code for the policy.
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the policy.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the policy.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Type of policy.
    /// </summary>
    public PolicyType Type { get; set; }

    /// <summary>
    /// Numeric value for the policy (e.g., minimum hours, maximum passengers).
    /// </summary>
    public int Value { get; set; }

    /// <summary>
    /// Secondary value if needed (e.g., for range-based policies).
    /// </summary>
    public int? SecondaryValue { get; set; }

    /// <summary>
    /// Error message to display when policy is violated.
    /// </summary>
    public string ErrorMessage { get; set; } = string.Empty;

    // Scope restrictions

    /// <summary>
    /// Specific airline ID (null = all airlines).
    /// </summary>
    public Guid? AirlineId { get; set; }

    /// <summary>
    /// Specific departure airport ID (null = all airports).
    /// </summary>
    public Guid? DepartureAirportId { get; set; }

    /// <summary>
    /// Specific arrival airport ID (null = all airports).
    /// </summary>
    public Guid? ArrivalAirportId { get; set; }

    /// <summary>
    /// Specific cabin class (null = all classes).
    /// </summary>
    public FlightClass? CabinClass { get; set; }

    /// <summary>
    /// Priority for applying policies (higher = applied first).
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Indicates if the policy is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Airline? Airline { get; set; }
    public Airport? DepartureAirport { get; set; }
    public Airport? ArrivalAirport { get; set; }
}

