namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the status of a booking segment (flight leg).
/// </summary>
public enum SegmentStatus
{
    /// <summary>
    /// Segment is confirmed and waiting for check-in.
    /// </summary>
    Confirmed = 0,

    /// <summary>
    /// Passenger has checked in for this segment.
    /// </summary>
    CheckedIn = 1,

    /// <summary>
    /// Passenger has boarded the aircraft.
    /// </summary>
    Boarded = 2,

    /// <summary>
    /// Flight segment has been completed.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Segment has been cancelled.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Passenger did not show up for this segment.
    /// </summary>
    NoShow = 5
}

