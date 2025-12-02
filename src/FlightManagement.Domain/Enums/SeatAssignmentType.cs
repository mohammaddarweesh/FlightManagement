namespace FlightManagement.Domain.Enums;

/// <summary>
/// Indicates how a seat was assigned to a passenger.
/// </summary>
public enum SeatAssignmentType
{
    /// <summary>
    /// Passenger selected the seat during booking.
    /// </summary>
    Selected = 0,

    /// <summary>
    /// System automatically assigned the seat.
    /// </summary>
    AutoAssigned = 1,

    /// <summary>
    /// Seat was assigned during check-in.
    /// </summary>
    CheckInAssigned = 2
}

