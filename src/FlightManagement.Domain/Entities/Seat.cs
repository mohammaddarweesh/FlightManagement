using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Represents an individual physical seat on an aircraft.
/// Contains seat location, type, and special features.
/// </summary>
public class Seat : BaseEntity
{
    /// <summary>
    /// Foreign key to the aircraft.
    /// </summary>
    public Guid AircraftId { get; set; }

    /// <summary>
    /// Seat identifier combining row and column.
    /// Example: "1A", "12F", "32C"
    /// </summary>
    public string SeatNumber { get; set; } = string.Empty;

    /// <summary>
    /// Row number of the seat. Example: 1, 12, 32
    /// </summary>
    public int Row { get; set; }

    /// <summary>
    /// Column letter of the seat. Example: 'A', 'B', 'F'
    /// </summary>
    public char Column { get; set; }

    /// <summary>
    /// The cabin class this seat belongs to.
    /// </summary>
    public FlightClass CabinClass { get; set; }

    /// <summary>
    /// Position type of the seat (Window, Middle, Aisle).
    /// </summary>
    public SeatType SeatType { get; set; }

    /// <summary>
    /// Indicates if this seat is located at an emergency exit row.
    /// Usually has extra legroom but may have restrictions.
    /// </summary>
    public bool IsEmergencyExit { get; set; }

    /// <summary>
    /// Indicates if this seat has extra legroom.
    /// May command a premium price.
    /// </summary>
    public bool HasExtraLegroom { get; set; }

    /// <summary>
    /// Indicates if this seat is available for booking.
    /// False for broken seats, crew seats, etc.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties

    /// <summary>
    /// The aircraft this seat belongs to.
    /// </summary>
    public Aircraft Aircraft { get; set; } = null!;

    /// <summary>
    /// Flight-specific seat availability records.
    /// </summary>
    public ICollection<FlightSeat> FlightSeats { get; set; } = new List<FlightSeat>();
}

