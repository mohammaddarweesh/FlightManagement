using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Defines airline overbooking policies to allow controlled overbooking.
/// Airlines often overbook to account for no-shows while managing risk.
/// </summary>
public class OverbookingPolicy : BaseEntity
{
    /// <summary>
    /// Foreign key to the airline.
    /// </summary>
    public Guid AirlineId { get; set; }

    /// <summary>
    /// Display name of the policy.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the policy.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Maximum overbooking percentage allowed (e.g., 5 = 5% overbooking).
    /// </summary>
    public decimal MaxOverbookingPercentage { get; set; }

    /// <summary>
    /// Maximum absolute number of overbooked seats allowed.
    /// </summary>
    public int? MaxOverbookedSeats { get; set; }

    /// <summary>
    /// Cabin class this policy applies to (null = all classes).
    /// </summary>
    public FlightClass? CabinClass { get; set; }

    /// <summary>
    /// Specific route departure airport (null = all routes).
    /// </summary>
    public Guid? DepartureAirportId { get; set; }

    /// <summary>
    /// Specific route arrival airport (null = all routes).
    /// </summary>
    public Guid? ArrivalAirportId { get; set; }

    /// <summary>
    /// Priority for applying policies (higher = applied first).
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Indicates if the policy is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Airline Airline { get; set; } = null!;
    public Airport? DepartureAirport { get; set; }
    public Airport? ArrivalAirport { get; set; }
}

