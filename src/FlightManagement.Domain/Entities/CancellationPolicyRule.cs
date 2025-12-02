using FlightManagement.Domain.Common;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Defines a specific refund tier within a cancellation policy.
/// </summary>
public class CancellationPolicyRule : BaseEntity
{
    /// <summary>
    /// Foreign key to the cancellation policy.
    /// </summary>
    public Guid CancellationPolicyId { get; set; }

    /// <summary>
    /// Minimum hours before departure for this tier to apply.
    /// </summary>
    public int MinHoursBeforeDeparture { get; set; }

    /// <summary>
    /// Maximum hours before departure for this tier (null = no max).
    /// </summary>
    public int? MaxHoursBeforeDeparture { get; set; }

    /// <summary>
    /// Percentage of fare refunded (0-100).
    /// </summary>
    public decimal RefundPercentage { get; set; }

    /// <summary>
    /// Flat cancellation fee charged.
    /// </summary>
    public decimal FlatFee { get; set; }

    /// <summary>
    /// Currency for the flat fee.
    /// </summary>
    public string Currency { get; set; } = "USD";

    // Navigation properties

    public CancellationPolicy CancellationPolicy { get; set; } = null!;
}

