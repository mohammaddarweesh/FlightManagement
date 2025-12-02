using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Defines dates when certain bookings or promotions are not allowed.
/// Used for high-demand periods where special restrictions apply.
/// </summary>
public class BlackoutDate : BaseEntity
{
    /// <summary>
    /// Display name of the blackout period.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description/reason for the blackout.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Start date of the blackout period.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the blackout period.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// If true, blocks all bookings. If false, only blocks promotions.
    /// </summary>
    public bool BlocksBookings { get; set; }

    /// <summary>
    /// If true, promotions cannot be used during this period.
    /// </summary>
    public bool BlocksPromotions { get; set; } = true;

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
    /// Indicates if this blackout period is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Airline? Airline { get; set; }
    public Airport? DepartureAirport { get; set; }
    public Airport? ArrivalAirport { get; set; }
}

