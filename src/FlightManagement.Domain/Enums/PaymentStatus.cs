namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the status of a payment transaction.
/// </summary>
public enum PaymentStatus
{
    /// <summary>
    /// Payment is pending processing.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Payment is being processed.
    /// </summary>
    Processing = 1,

    /// <summary>
    /// Payment completed successfully.
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Payment failed.
    /// </summary>
    Failed = 3,

    /// <summary>
    /// Payment was cancelled.
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Payment was refunded.
    /// </summary>
    Refunded = 5,

    /// <summary>
    /// Payment is disputed.
    /// </summary>
    Disputed = 6
}

