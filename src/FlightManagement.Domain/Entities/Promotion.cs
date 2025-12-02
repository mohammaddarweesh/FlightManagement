using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Represents a promotional discount that can be applied to bookings.
/// Supports various promotion types with usage limits and date restrictions.
/// </summary>
public class Promotion : BaseEntity
{
    /// <summary>
    /// Unique promotion code (e.g., "SUMMER2024", "WELCOME10").
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the promotion.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the promotion.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Type of promotion.
    /// </summary>
    public PromotionType Type { get; set; }

    /// <summary>
    /// How the discount is calculated.
    /// </summary>
    public DiscountType DiscountType { get; set; }

    /// <summary>
    /// Discount value (percentage 0-100 or fixed amount).
    /// </summary>
    public decimal DiscountValue { get; set; }

    /// <summary>
    /// Maximum discount amount (for percentage discounts).
    /// </summary>
    public decimal? MaxDiscountAmount { get; set; }

    /// <summary>
    /// Minimum booking amount required to use this promotion.
    /// </summary>
    public decimal? MinBookingAmount { get; set; }

    /// <summary>
    /// Currency for monetary values.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Current status of the promotion.
    /// </summary>
    public PromotionStatus Status { get; set; } = PromotionStatus.Draft;

    // Date restrictions

    /// <summary>
    /// When the promotion becomes active.
    /// </summary>
    public DateTime ValidFrom { get; set; }

    /// <summary>
    /// When the promotion expires.
    /// </summary>
    public DateTime ValidTo { get; set; }

    // Usage limits

    /// <summary>
    /// Maximum total uses across all customers (null = unlimited).
    /// </summary>
    public int? MaxTotalUses { get; set; }

    /// <summary>
    /// Maximum uses per customer (null = unlimited).
    /// </summary>
    public int? MaxUsesPerCustomer { get; set; }

    /// <summary>
    /// Current total usage count.
    /// </summary>
    public int CurrentUsageCount { get; set; }

    // Applicable restrictions

    /// <summary>
    /// Specific routes this promotion applies to (null = all routes).
    /// Stored as JSON array of route pairs, e.g., [{"from":"JFK","to":"LAX"}]
    /// </summary>
    public string? ApplicableRoutes { get; set; }

    /// <summary>
    /// Specific cabin classes this promotion applies to (null = all classes).
    /// </summary>
    public string? ApplicableCabinClasses { get; set; }

    /// <summary>
    /// Days of week the promotion is valid.
    /// </summary>
    public DayOfWeekFlag ApplicableDays { get; set; } = DayOfWeekFlag.AllDays;

    /// <summary>
    /// Specific airline IDs this promotion applies to.
    /// </summary>
    public string? ApplicableAirlineIds { get; set; }

    /// <summary>
    /// First-time customers only.
    /// </summary>
    public bool FirstTimeCustomersOnly { get; set; }

    /// <summary>
    /// Indicates if the promotion is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<PromotionUsage> Usages { get; set; } = new List<PromotionUsage>();
}

