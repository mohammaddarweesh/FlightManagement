using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Audit trail for booking modifications.
/// </summary>
public class BookingHistory : BaseEntity
{
    /// <summary>
    /// Foreign key to the booking.
    /// </summary>
    public Guid BookingId { get; set; }

    /// <summary>
    /// Action that was performed.
    /// </summary>
    public BookingAction Action { get; set; }

    /// <summary>
    /// Human-readable description of the action.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Previous state/values (JSON).
    /// </summary>
    public string? OldValues { get; set; }

    /// <summary>
    /// New state/values (JSON).
    /// </summary>
    public string? NewValues { get; set; }

    /// <summary>
    /// User ID who performed the action (null for system).
    /// </summary>
    public Guid? PerformedBy { get; set; }

    /// <summary>
    /// Type of actor who performed the action.
    /// </summary>
    public ActorType PerformedByType { get; set; }

    /// <summary>
    /// When the action was performed.
    /// </summary>
    public DateTime PerformedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// IP address of the client.
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent of the client.
    /// </summary>
    public string? UserAgent { get; set; }

    // Navigation properties

    public Booking Booking { get; set; } = null!;
}

