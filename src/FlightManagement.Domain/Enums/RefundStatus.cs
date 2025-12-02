namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the status of a refund request.
/// </summary>
public enum RefundStatus
{
    /// <summary>
    /// Refund is pending processing.
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Refund has been approved.
    /// </summary>
    Approved = 1,

    /// <summary>
    /// Refund is being processed.
    /// </summary>
    Processing = 2,

    /// <summary>
    /// Refund completed successfully.
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Refund was rejected.
    /// </summary>
    Rejected = 4,

    /// <summary>
    /// Refund failed.
    /// </summary>
    Failed = 5
}

