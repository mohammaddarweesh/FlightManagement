using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Stores pricing information for a specific cabin class on a flight.
/// Supports dynamic pricing and availability tracking.
/// </summary>
public class FlightPricing : BaseEntity
{
    /// <summary>
    /// Foreign key to the flight.
    /// </summary>
    public Guid FlightId { get; set; }

    /// <summary>
    /// The cabin class this pricing applies to.
    /// </summary>
    public FlightClass CabinClass { get; set; }

    /// <summary>
    /// Base ticket price before dynamic adjustments.
    /// </summary>
    public decimal BasePrice { get; set; }

    /// <summary>
    /// Current selling price (may differ from base due to demand).
    /// </summary>
    public decimal CurrentPrice { get; set; }

    /// <summary>
    /// Currency code for the prices. Example: "USD", "EUR", "GBP"
    /// </summary>
    public string Currency { get; set; } = "USD";

    /// <summary>
    /// Tax amount per ticket.
    /// </summary>
    public decimal TaxAmount { get; set; }

    /// <summary>
    /// Total seats available in this class for this flight.
    /// </summary>
    public int TotalSeats { get; set; }

    /// <summary>
    /// Remaining seats available for booking.
    /// Decrements on booking, increments on cancellation.
    /// </summary>
    public int AvailableSeats { get; set; }

    /// <summary>
    /// Indicates if this pricing tier is active and bookable.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties

    /// <summary>
    /// The flight this pricing belongs to.
    /// </summary>
    public Flight Flight { get; set; } = null!;

    // Calculated properties

    /// <summary>
    /// Total price including tax.
    /// </summary>
    public decimal TotalPrice => CurrentPrice + TaxAmount;

    /// <summary>
    /// Percentage of seats already booked.
    /// Used for dynamic pricing calculations.
    /// </summary>
    public decimal BookingPercentage => TotalSeats > 0 
        ? (decimal)(TotalSeats - AvailableSeats) / TotalSeats * 100 
        : 0;
}

