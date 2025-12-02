namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the current operational status of a flight.
/// Tracks the flight lifecycle from scheduled to arrived.
/// </summary>
public enum FlightStatus
{
    /// <summary>
    /// Flight is scheduled and confirmed. Default initial state.
    /// </summary>
    Scheduled = 0,

    /// <summary>
    /// Passengers are currently boarding the aircraft.
    /// </summary>
    Boarding = 1,

    /// <summary>
    /// Aircraft has left the gate and departed.
    /// </summary>
    Departed = 2,

    /// <summary>
    /// Aircraft is currently in flight.
    /// </summary>
    InFlight = 3,

    /// <summary>
    /// Aircraft has landed at destination airport.
    /// </summary>
    Landed = 4,

    /// <summary>
    /// Flight has arrived and passengers have disembarked.
    /// </summary>
    Arrived = 5,

    /// <summary>
    /// Flight is delayed from its scheduled time.
    /// </summary>
    Delayed = 6,

    /// <summary>
    /// Flight has been cancelled and will not operate.
    /// </summary>
    Cancelled = 7
}

