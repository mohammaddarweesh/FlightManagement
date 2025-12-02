namespace FlightManagement.Domain.Enums;

/// <summary>
/// Types of promotions available in the system.
/// </summary>
public enum PromotionType
{
    /// <summary>
    /// General promotional discount (e.g., seasonal sale).
    /// </summary>
    General = 0,

    /// <summary>
    /// Route-specific promotion.
    /// </summary>
    RouteSpecific = 1,

    /// <summary>
    /// First-time booking discount.
    /// </summary>
    FirstBooking = 2,

    /// <summary>
    /// Referral discount code.
    /// </summary>
    Referral = 3,

    /// <summary>
    /// Loyalty program reward.
    /// </summary>
    Loyalty = 4,

    /// <summary>
    /// Flash sale with limited duration.
    /// </summary>
    FlashSale = 5,

    /// <summary>
    /// Early bird booking discount.
    /// </summary>
    EarlyBird = 6,

    /// <summary>
    /// Last-minute deal discount.
    /// </summary>
    LastMinute = 7,

    /// <summary>
    /// Group booking discount.
    /// </summary>
    GroupBooking = 8,

    /// <summary>
    /// Corporate/business discount.
    /// </summary>
    Corporate = 9
}

