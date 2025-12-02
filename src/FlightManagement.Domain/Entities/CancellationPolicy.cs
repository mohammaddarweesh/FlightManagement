using FlightManagement.Domain.Common;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Defines cancellation and refund rules for bookings.
/// </summary>
public class CancellationPolicy : BaseEntity
{
    /// <summary>
    /// Unique code for the policy (e.g., "FLEX", "STANDARD", "NONREF").
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the policy.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Full description of the policy terms.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Indicates if any refund is possible under this policy.
    /// </summary>
    public bool IsRefundable { get; set; }

    /// <summary>
    /// Indicates if this policy is currently active.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties

    public ICollection<CancellationPolicyRule> Rules { get; set; } = new List<CancellationPolicyRule>();
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

