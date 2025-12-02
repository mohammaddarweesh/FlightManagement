namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the current status of a booking throughout its lifecycle.
/// </summary>
public enum BookingStatus
{
    /// <summary>
    /// Booking is created but not yet confirmed/paid.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Payment received and booking is confirmed.
    /// </summary>
    Confirmed = 1,

    /// <summary>
    /// Tickets have been issued for the booking.
    /// </summary>
    Ticketed = 2,

    /// <summary>
    /// All flights in the booking have been completed.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Booking has been cancelled by customer or airline.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Booking expired before payment was completed.
    /// </summary>
    Expired = 5,

    /// <summary>
    /// Passenger did not show up for the flight.
    /// </summary>
    NoShow = 6
}

