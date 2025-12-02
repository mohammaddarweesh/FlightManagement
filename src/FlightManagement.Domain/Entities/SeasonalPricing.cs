using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Defines seasonal pricing periods with specific date ranges.
/// </summary>
public class SeasonalPricing : BaseEntity
{
    /// <summary>
    /// Name of the seasonal period (e.g., "Summer 2024", "Christmas Peak").
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the season.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Type of season for pricing.
    /// </summary>
    public SeasonType SeasonType { get; set; }

    /// <summary>
    /// Start date of the seasonal period.
    /// </summary>
    public DateTime StartDate { get; set; }

    /// <summary>
    /// End date of the seasonal period.
    /// </summary>
    public DateTime EndDate { get; set; }

    /// <summary>
    /// Price adjustment percentage (can be negative for low season).
    /// E.g., 20 = 20% increase, -15 = 15% discount.
    /// </summary>
    public decimal AdjustmentPercentage { get; set; }

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
    /// Priority for applying seasonal pricing (higher = applied first).
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// Indicates if this seasonal pricing is active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Airline? Airline { get; set; }
    public Airport? DepartureAirport { get; set; }
    public Airport? ArrivalAirport { get; set; }
}

