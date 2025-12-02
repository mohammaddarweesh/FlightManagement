namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the status of a booking extra/add-on.
/// </summary>
public enum ExtraStatus
{
    /// <summary>
    /// Extra is pending confirmation.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Extra has been confirmed.
    /// </summary>
    Confirmed = 1,

    /// <summary>
    /// Extra has been cancelled.
    /// </summary>
    Cancelled = 2,

    /// <summary>
    /// Extra has been used/consumed.
    /// </summary>
    Used = 3,

    /// <summary>
    /// Extra has been refunded.
    /// </summary>
    Refunded = 4
}

