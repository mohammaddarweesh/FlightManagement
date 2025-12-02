namespace FlightManagement.Domain.Enums;

/// <summary>
/// Season types for seasonal pricing adjustments.
/// </summary>
public enum SeasonType
{
    /// <summary>
    /// Regular/off-peak season.
    /// </summary>
    Regular = 0,

    /// <summary>
    /// Low season with lower demand.
    /// </summary>
    Low = 1,

    /// <summary>
    /// High/peak season with increased demand.
    /// </summary>
    Peak = 2,

    /// <summary>
    /// Holiday period with premium pricing.
    /// </summary>
    Holiday = 3
}

