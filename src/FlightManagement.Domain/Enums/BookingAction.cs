namespace FlightManagement.Domain.Enums;

/// <summary>
/// Actions that can be performed on a booking for audit trail.
/// </summary>
public enum BookingAction
{
    /// <summary>
    /// Booking was created.
    /// </summary>
    Created = 0,

    /// <summary>
    /// Passenger was added.
    /// </summary>
    PassengerAdded = 1,

    /// <summary>
    /// Passenger was updated.
    /// </summary>
    PassengerUpdated = 2,

    /// <summary>
    /// Passenger was removed.
    /// </summary>
    PassengerRemoved = 3,

    /// <summary>
    /// Seat was selected.
    /// </summary>
    SeatSelected = 4,

    /// <summary>
    /// Seat was changed.
    /// </summary>
    SeatChanged = 5,

    /// <summary>
    /// Extra was added.
    /// </summary>
    ExtraAdded = 6,

    /// <summary>
    /// Extra was removed.
    /// </summary>
    ExtraRemoved = 7,

    /// <summary>
    /// Payment was received.
    /// </summary>
    PaymentReceived = 8,

    /// <summary>
    /// Booking was confirmed.
    /// </summary>
    Confirmed = 9,

    /// <summary>
    /// Booking was ticketed.
    /// </summary>
    Ticketed = 10,

    /// <summary>
    /// Flight date was changed.
    /// </summary>
    DateChanged = 11,

    /// <summary>
    /// Cabin class was upgraded.
    /// </summary>
    ClassUpgraded = 12,

    /// <summary>
    /// Booking was cancelled.
    /// </summary>
    Cancelled = 13,

    /// <summary>
    /// Refund was processed.
    /// </summary>
    RefundProcessed = 14,

    /// <summary>
    /// Check-in was completed.
    /// </summary>
    CheckedIn = 15,

    /// <summary>
    /// Booking was completed.
    /// </summary>
    Completed = 16,

    /// <summary>
    /// Contact information was updated.
    /// </summary>
    ContactUpdated = 17,

    /// <summary>
    /// Special request was added/updated.
    /// </summary>
    SpecialRequestUpdated = 18
}

