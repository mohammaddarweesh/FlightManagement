namespace FlightManagement.Domain.Enums;

/// <summary>
/// Status of a promotion.
/// </summary>
public enum PromotionStatus
{
    /// <summary>
    /// Promotion is in draft mode, not yet active.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// Promotion is active and can be used.
    /// </summary>
    Active = 1,

    /// <summary>
    /// Promotion has been paused.
    /// </summary>
    Paused = 2,

    /// <summary>
    /// Promotion has expired.
    /// </summary>
    Expired = 3,

    /// <summary>
    /// Promotion usage limit has been reached.
    /// </summary>
    Exhausted = 4,

    /// <summary>
    /// Promotion has been cancelled/deactivated.
    /// </summary>
    Cancelled = 5
}

