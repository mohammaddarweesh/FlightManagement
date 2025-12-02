namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the position of a seat within a row.
/// Used for seat selection preferences and pricing.
/// </summary>
public enum SeatType
{
    /// <summary>
    /// Seat next to the window. Preferred for views and wall to lean on.
    /// </summary>
    Window = 0,

    /// <summary>
    /// Seat between window and aisle seats. Least preferred position.
    /// </summary>
    Middle = 1,

    /// <summary>
    /// Seat next to the aisle. Easy access for movement.
    /// </summary>
    Aisle = 2
}

