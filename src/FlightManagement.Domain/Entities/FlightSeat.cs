using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Tracks individual seat availability for a specific flight.
/// Enables seat selection, temporary reservations, and booking management.
/// </summary>
public class FlightSeat : BaseEntity
{
    /// <summary>
    /// Foreign key to the flight.
    /// </summary>
    public Guid FlightId { get; set; }

    /// <summary>
    /// Foreign key to the physical seat.
    /// </summary>
    public Guid SeatId { get; set; }

    /// <summary>
    /// Current availability status of this seat.
    /// </summary>
    public SeatStatus Status { get; set; } = SeatStatus.Available;

    /// <summary>
    /// Seat-specific price if different from class base price.
    /// Used for premium seats (exit row, extra legroom).
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Expiry time for temporary reservation.
    /// After this time, the seat can be released back to available.
    /// </summary>
    public DateTime? LockedUntil { get; set; }

    /// <summary>
    /// User ID who currently holds the reservation lock.
    /// </summary>
    public Guid? LockedByUserId { get; set; }

    /// <summary>
    /// Foreign key to the confirmed booking (when status is Booked).
    /// </summary>
    public Guid? BookingId { get; set; }

    // Navigation properties

    /// <summary>
    /// The flight this seat availability belongs to.
    /// </summary>
    public Flight Flight { get; set; } = null!;

    /// <summary>
    /// The physical seat definition.
    /// </summary>
    public Seat Seat { get; set; } = null!;

    /// <summary>
    /// The booking that has reserved/booked this seat.
    /// </summary>
    public Booking? Booking { get; set; }

    /// <summary>
    /// Passenger seat assignments for this flight seat.
    /// </summary>
    public ICollection<PassengerSeat> PassengerSeats { get; set; } = new List<PassengerSeat>();

    // Helper methods

    /// <summary>
    /// Checks if the reservation lock has expired.
    /// </summary>
    public bool IsLockExpired => LockedUntil.HasValue && LockedUntil.Value < DateTime.UtcNow;

    /// <summary>
    /// Checks if the seat can be reserved (available or expired lock).
    /// </summary>
    public bool CanBeReserved => Status == SeatStatus.Available || 
        (Status == SeatStatus.Reserved && IsLockExpired);
}

