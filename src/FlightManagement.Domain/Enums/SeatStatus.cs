namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the availability status of a seat on a specific flight.
/// Used for seat inventory management and booking flow.
/// </summary>
public enum SeatStatus
{
    /// <summary>
    /// Seat is available for booking.
    /// </summary>
    Available = 0,

    /// <summary>
    /// Seat is temporarily held during the booking process.
    /// Will be released if booking is not completed within the lock period.
    /// </summary>
    Reserved = 1,

    /// <summary>
    /// Seat has been confirmed and paid for.
    /// Associated with a completed booking.
    /// </summary>
    Booked = 2,

    /// <summary>
    /// Seat is blocked by the airline (crew seats, broken seats, etc.).
    /// Not available for passenger booking.
    /// </summary>
    Blocked = 3,

    /// <summary>
    /// Seat is not available for sale (structural limitations, etc.).
    /// </summary>
    Unavailable = 4
}

