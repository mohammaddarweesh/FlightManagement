namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the cabin class tiers on a flight.
/// Each class offers different service levels, seat configurations, and pricing.
/// </summary>
public enum FlightClass
{
    /// <summary>
    /// Standard class with basic amenities. Most affordable option.
    /// Typical seat configuration: 3-3 or 3-4-3
    /// </summary>
    Economy = 0,

    /// <summary>
    /// Enhanced economy with extra legroom and improved services.
    /// Typical seat configuration: 2-4-2 or 2-3-2
    /// </summary>
    PremiumEconomy = 1,

    /// <summary>
    /// Premium class with lie-flat seats, premium dining, and priority services.
    /// Typical seat configuration: 2-2 or 1-2-1
    /// </summary>
    Business = 2,

    /// <summary>
    /// Luxury class with private suites, exclusive services, and maximum comfort.
    /// Typical seat configuration: 1-1 or 1-2-1
    /// </summary>
    First = 3
}

