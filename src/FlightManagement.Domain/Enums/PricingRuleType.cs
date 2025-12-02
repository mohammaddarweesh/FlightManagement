namespace FlightManagement.Domain.Enums;

/// <summary>
/// Types of dynamic pricing rules.
/// </summary>
public enum PricingRuleType
{
    /// <summary>
    /// Day of week pricing adjustment (e.g., weekend surcharge).
    /// </summary>
    DayOfWeek = 0,

    /// <summary>
    /// Seasonal pricing adjustment.
    /// </summary>
    Seasonal = 1,

    /// <summary>
    /// Demand-based pricing (based on booking percentage).
    /// </summary>
    DemandBased = 2,

    /// <summary>
    /// Advance purchase discount.
    /// </summary>
    AdvancePurchase = 3,

    /// <summary>
    /// Last-minute pricing adjustment.
    /// </summary>
    LastMinute = 4,

    /// <summary>
    /// Time of day pricing (e.g., red-eye discount).
    /// </summary>
    TimeOfDay = 5,

    /// <summary>
    /// Route-specific pricing adjustment.
    /// </summary>
    RouteSpecific = 6
}

