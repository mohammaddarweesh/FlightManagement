namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the type of journey for a booking.
/// </summary>
public enum TripType
{
    /// <summary>
    /// Single direction flight only.
    /// </summary>
    OneWay = 0,

    /// <summary>
    /// Outbound and return flights.
    /// </summary>
    RoundTrip = 1,

    /// <summary>
    /// Multiple destinations with different stops.
    /// </summary>
    MultiCity = 2
}

