using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Represents a single flight leg within a booking.
/// Enables multi-leg journeys (round trips, connections) with per-segment pricing.
/// </summary>
public class BookingSegment : BaseEntity
{
    /// <summary>
    /// Foreign key to the booking.
    /// </summary>
    public Guid BookingId { get; set; }

    /// <summary>
    /// Foreign key to the flight.
    /// </summary>
    public Guid FlightId { get; set; }

    /// <summary>
    /// Order of this segment in the journey (1 = outbound, 2 = return, etc.).
    /// </summary>
    public int SegmentOrder { get; set; }

    /// <summary>
    /// Cabin class for this segment.
    /// </summary>
    public FlightClass CabinClass { get; set; }

    // Pricing

    /// <summary>
    /// Base fare per passenger for this segment.
    /// </summary>
    public decimal BaseFarePerPax { get; set; }

    /// <summary>
    /// Tax per passenger for this segment.
    /// </summary>
    public decimal TaxPerPax { get; set; }

    /// <summary>
    /// Total subtotal for this segment (all passengers).
    /// </summary>
    public decimal SegmentSubtotal { get; set; }

    // Status

    /// <summary>
    /// Current status of this segment.
    /// </summary>
    public SegmentStatus Status { get; set; } = SegmentStatus.Confirmed;

    /// <summary>
    /// When check-in opens for this segment.
    /// </summary>
    public DateTime? CheckInOpenAt { get; set; }

    // Baggage allowance

    /// <summary>
    /// Included checked baggage allowance in kg.
    /// </summary>
    public int CheckedBaggageAllowanceKg { get; set; }

    /// <summary>
    /// Included cabin baggage allowance in kg.
    /// </summary>
    public int CabinBaggageAllowanceKg { get; set; }

    // Navigation properties

    public Booking Booking { get; set; } = null!;
    public Flight Flight { get; set; } = null!;
    public ICollection<PassengerSeat> PassengerSeats { get; set; } = new List<PassengerSeat>();
}

