using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Represents a flight reservation containing passengers, segments, and pricing.
/// Links a customer to one or more flights with complete price breakdown.
/// </summary>
public class Booking : BaseEntity
{
    /// <summary>
    /// Unique 6-character booking reference (PNR). Example: "XY7K2M"
    /// </summary>
    public string BookingReference { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to the customer making the booking.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Current status of the booking.
    /// </summary>
    public BookingStatus Status { get; set; } = BookingStatus.Pending;

    /// <summary>
    /// When the booking was created.
    /// </summary>
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Expiration time for pending bookings awaiting payment.
    /// </summary>
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Type of trip (OneWay, RoundTrip, MultiCity).
    /// </summary>
    public TripType TripType { get; set; }

    // Pricing breakdown

    /// <summary>
    /// Sum of base fares for all segments and passengers.
    /// </summary>
    public decimal BaseFare { get; set; }

    /// <summary>
    /// Total tax amount.
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// Platform service fee.
    /// </summary>
    public decimal ServiceFee { get; set; }

    /// <summary>
    /// Total seat selection fees.
    /// </summary>
    public decimal SeatSelectionFees { get; set; }

    /// <summary>
    /// Total extras/add-ons fees.
    /// </summary>
    public decimal ExtrasFees { get; set; }

    /// <summary>
    /// Discount amount applied (promo codes, etc.).
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Final total amount payable.
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Currency code (e.g., "USD", "EUR").
    /// </summary>
    public string Currency { get; set; } = "USD";

    // Contact information

    /// <summary>
    /// Primary contact email for booking notifications.
    /// </summary>
    public string ContactEmail { get; set; } = string.Empty;

    /// <summary>
    /// Primary contact phone number.
    /// </summary>
    public string ContactPhone { get; set; } = string.Empty;

    /// <summary>
    /// Special requests or notes for the booking.
    /// </summary>
    public string? SpecialRequests { get; set; }

    // Payment information

    /// <summary>
    /// Current payment status.
    /// </summary>
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    /// <summary>
    /// Amount paid so far.
    /// </summary>
    public decimal PaidAmount { get; set; }

    /// <summary>
    /// Payment deadline for pending bookings.
    /// </summary>
    public DateTime? PaymentDueDate { get; set; }

    // Cancellation information

    /// <summary>
    /// Foreign key to the cancellation policy.
    /// </summary>
    public Guid? CancellationPolicyId { get; set; }

    /// <summary>
    /// When the booking was cancelled.
    /// </summary>
    public DateTime? CancelledAt { get; set; }

    /// <summary>
    /// Reason for cancellation.
    /// </summary>
    public string? CancellationReason { get; set; }

    /// <summary>
    /// Amount refunded after cancellation.
    /// </summary>
    public decimal? RefundAmount { get; set; }

    /// <summary>
    /// Status of the refund.
    /// </summary>
    public RefundStatus? RefundStatus { get; set; }

    /// <summary>
    /// Promo code applied to the booking.
    /// </summary>
    public string? PromoCode { get; set; }

    /// <summary>
    /// Foreign key to the promotion used (if any).
    /// </summary>
    public Guid? PromotionId { get; set; }

    // Navigation properties

    public Customer Customer { get; set; } = null!;
    public CancellationPolicy? CancellationPolicy { get; set; }
    public Promotion? Promotion { get; set; }
    public ICollection<Passenger> Passengers { get; set; } = new List<Passenger>();
    public ICollection<BookingSegment> Segments { get; set; } = new List<BookingSegment>();
    public ICollection<BookingExtra> Extras { get; set; } = new List<BookingExtra>();
    public ICollection<PaymentRecord> PaymentRecords { get; set; } = new List<PaymentRecord>();
    public ICollection<BookingHistory> History { get; set; } = new List<BookingHistory>();
    public ICollection<PromotionUsage> PromotionUsages { get; set; } = new List<PromotionUsage>();
}

