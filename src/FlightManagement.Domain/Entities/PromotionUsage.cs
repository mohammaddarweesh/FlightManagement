using FlightManagement.Domain.Common;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Tracks the usage of promotions by customers.
/// Used to enforce per-customer usage limits.
/// </summary>
public class PromotionUsage : BaseEntity
{
    /// <summary>
    /// Foreign key to the promotion.
    /// </summary>
    public Guid PromotionId { get; set; }

    /// <summary>
    /// Foreign key to the customer who used the promotion.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Foreign key to the booking where the promotion was applied.
    /// </summary>
    public Guid BookingId { get; set; }

    /// <summary>
    /// Discount amount applied.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Currency of the discount.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// When the promotion was used.
    /// </summary>
    public DateTime UsedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Promotion Promotion { get; set; } = null!;
    public Customer Customer { get; set; } = null!;
    public Booking Booking { get; set; } = null!;
}

