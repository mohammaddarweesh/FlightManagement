namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the type of user in the system
/// </summary>
public enum UserType
{
    /// <summary>
    /// Regular customer user
    /// </summary>
    Customer = 0,

    /// <summary>
    /// Administrator with full access
    /// </summary>
    Admin = 1,

    /// <summary>
    /// Staff user with limited admin access (reports, view bookings)
    /// </summary>
    Staff = 2
}

