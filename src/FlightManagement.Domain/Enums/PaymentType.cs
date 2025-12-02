namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the type of payment transaction.
/// </summary>
public enum PaymentType
{
    /// <summary>
    /// Initial payment for booking.
    /// </summary>
    Initial = 0,

    /// <summary>
    /// Additional payment (e.g., for upgrades).
    /// </summary>
    Additional = 1,

    /// <summary>
    /// Refund to customer.
    /// </summary>
    Refund = 2,

    /// <summary>
    /// Chargeback initiated by customer.
    /// </summary>
    Chargeback = 3,

    /// <summary>
    /// Partial payment.
    /// </summary>
    Partial = 4,

    /// <summary>
    /// Compensation payment.
    /// </summary>
    Compensation = 5
}

