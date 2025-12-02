namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the age category of a passenger.
/// Affects pricing and seat requirements.
/// </summary>
public enum PassengerType
{
    /// <summary>
    /// Passenger aged 12 years or older.
    /// </summary>
    Adult = 0,

    /// <summary>
    /// Passenger aged 2-11 years.
    /// May receive discounted fare.
    /// </summary>
    Child = 1,

    /// <summary>
    /// Passenger under 2 years old.
    /// Typically travels on adult's lap or in bassinet.
    /// </summary>
    Infant = 2
}

