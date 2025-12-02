using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Links a passenger to their assigned seat on a specific flight segment.
/// Connects to the existing FlightSeat entity.
/// </summary>
public class PassengerSeat : BaseEntity
{
    /// <summary>
    /// Foreign key to the passenger.
    /// </summary>
    public Guid PassengerId { get; set; }

    /// <summary>
    /// Foreign key to the booking segment.
    /// </summary>
    public Guid BookingSegmentId { get; set; }

    /// <summary>
    /// Foreign key to the flight seat.
    /// </summary>
    public Guid FlightSeatId { get; set; }

    /// <summary>
    /// Denormalized seat number for easy access (e.g., "12A").
    /// </summary>
    public string SeatNumber { get; set; } = string.Empty;

    /// <summary>
    /// Fee paid for this seat selection.
    /// </summary>
    public decimal SeatFee { get; set; }

    /// <summary>
    /// When the seat was assigned.
    /// </summary>
    public DateTime AssignedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// How the seat was assigned.
    /// </summary>
    public SeatAssignmentType AssignmentType { get; set; }

    // Navigation properties

    public Passenger Passenger { get; set; } = null!;
    public BookingSegment BookingSegment { get; set; } = null!;
    public FlightSeat FlightSeat { get; set; } = null!;
}

