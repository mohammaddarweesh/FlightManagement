namespace FlightManagement.Domain.Enums;

/// <summary>
/// Type of discount calculation.
/// </summary>
public enum DiscountType
{
    /// <summary>
    /// Percentage discount off the base fare.
    /// </summary>
    Percentage = 0,

    /// <summary>
    /// Fixed amount discount.
    /// </summary>
    FixedAmount = 1,

    /// <summary>
    /// Fixed amount per passenger.
    /// </summary>
    PerPassenger = 2
}

