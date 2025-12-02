using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Tracks additional services purchased with the booking.
/// References existing FlightAmenity when applicable.
/// </summary>
public class BookingExtra : BaseEntity
{
    /// <summary>
    /// Foreign key to the booking.
    /// </summary>
    public Guid BookingId { get; set; }

    /// <summary>
    /// Foreign key to the segment (null = all segments).
    /// </summary>
    public Guid? BookingSegmentId { get; set; }

    /// <summary>
    /// Foreign key to the passenger (null = all passengers).
    /// </summary>
    public Guid? PassengerId { get; set; }

    /// <summary>
    /// Type of extra service.
    /// </summary>
    public ExtraType ExtraType { get; set; }

    /// <summary>
    /// Foreign key to the flight amenity (if applicable).
    /// </summary>
    public Guid? FlightAmenityId { get; set; }

    /// <summary>
    /// Description of the extra.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Quantity purchased.
    /// </summary>
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// Price per unit.
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Total price (Quantity × UnitPrice).
    /// </summary>
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// Currency code.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Current status of the extra.
    /// </summary>
    public ExtraStatus Status { get; set; } = ExtraStatus.Confirmed;

    // Navigation properties

    public Booking Booking { get; set; } = null!;
    public BookingSegment? BookingSegment { get; set; }
    public Passenger? Passenger { get; set; }
    public FlightAmenity? FlightAmenity { get; set; }
}

