using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Defines dynamic pricing adjustments based on various factors.
/// Supports day-of-week, seasonal, demand-based, and advance purchase pricing.
/// </summary>
public class DynamicPricingRule : BaseEntity
{
    /// <summary>
    /// Display name of the rule.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Description of the rule.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Type of pricing rule.
    /// </summary>
    public PricingRuleType RuleType { get; set; }

    /// <summary>
    /// Price adjustment as a percentage (can be negative for discounts).
    /// E.g., 15 = 15% increase, -10 = 10% discount.
    /// </summary>
    public decimal AdjustmentPercentage { get; set; }

    /// <summary>
    /// Fixed amount adjustment (alternative to percentage).
    /// </summary>
    public decimal? FixedAdjustment { get; set; }

    /// <summary>
    /// Currency for fixed adjustment.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Priority order (higher = applied first).
    /// </summary>
    public int Priority { get; set; }

    // Day of Week Rule parameters
    /// <summary>
    /// Days this rule applies to (for DayOfWeek type).
    /// </summary>
    public DayOfWeekFlag? ApplicableDays { get; set; }

    // Seasonal Rule parameters
    /// <summary>
    /// Season type (for Seasonal type).
    /// </summary>
    public SeasonType? SeasonType { get; set; }

    /// <summary>
    /// Start date for seasonal pricing.
    /// </summary>
    public DateTime? SeasonStartDate { get; set; }

    /// <summary>
    /// End date for seasonal pricing.
    /// </summary>
    public DateTime? SeasonEndDate { get; set; }

    // Demand-based Rule parameters
    /// <summary>
    /// Minimum booking percentage to trigger rule (for DemandBased type).
    /// </summary>
    public decimal? MinBookingPercentage { get; set; }

    /// <summary>
    /// Maximum booking percentage for rule to apply.
    /// </summary>
    public decimal? MaxBookingPercentage { get; set; }

    // Advance Purchase Rule parameters
    /// <summary>
    /// Minimum days before departure (for AdvancePurchase type).
    /// </summary>
    public int? MinDaysBeforeDeparture { get; set; }

    /// <summary>
    /// Maximum days before departure.
    /// </summary>
    public int? MaxDaysBeforeDeparture { get; set; }

    // Time of Day Rule parameters
    /// <summary>
    /// Start hour (0-23) for time-based pricing.
    /// </summary>
    public int? StartHour { get; set; }

    /// <summary>
    /// End hour (0-23) for time-based pricing.
    /// </summary>
    public int? EndHour { get; set; }

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
    /// Is this rule active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public Airline? Airline { get; set; }
    public Airport? DepartureAirport { get; set; }
    public Airport? ArrivalAirport { get; set; }
}

