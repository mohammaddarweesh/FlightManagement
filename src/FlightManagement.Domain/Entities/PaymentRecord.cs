using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Tracks all financial transactions for a booking.
/// </summary>
public class PaymentRecord : BaseEntity
{
    /// <summary>
    /// Foreign key to the booking.
    /// </summary>
    public Guid BookingId { get; set; }

    /// <summary>
    /// Unique transaction reference/ID.
    /// </summary>
    public string TransactionReference { get; set; } = string.Empty;

    /// <summary>
    /// Type of payment transaction.
    /// </summary>
    public PaymentType PaymentType { get; set; }

    /// <summary>
    /// Method of payment.
    /// </summary>
    public PaymentMethod PaymentMethod { get; set; }

    /// <summary>
    /// Transaction amount.
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Currency code.
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Current status of the payment.
    /// </summary>
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    /// <summary>
    /// When the payment was processed.
    /// </summary>
    public DateTime? ProcessedAt { get; set; }

    /// <summary>
    /// External payment gateway reference.
    /// </summary>
    public string? GatewayReference { get; set; }

    /// <summary>
    /// Gateway response data (JSON).
    /// </summary>
    public string? GatewayResponse { get; set; }

    /// <summary>
    /// Last 4 digits of card (if applicable).
    /// </summary>
    public string? CardLastFour { get; set; }

    /// <summary>
    /// Card brand (Visa, Mastercard, etc.).
    /// </summary>
    public string? CardBrand { get; set; }

    /// <summary>
    /// Reason for failure (if applicable).
    /// </summary>
    public string? FailureReason { get; set; }

    /// <summary>
    /// IP address of the payer.
    /// </summary>
    public string? IpAddress { get; set; }

    // Navigation properties

    public Booking Booking { get; set; } = null!;
}

