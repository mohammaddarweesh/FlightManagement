using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Bookings.Queries.CalculateBookingPrice;

/// <summary>
/// Query to calculate the total price for a booking.
/// </summary>
public record CalculateBookingPriceQuery(
    List<FlightSegmentPriceInput> Segments,
    int AdultCount,
    int ChildCount = 0,
    int InfantCount = 0,
    string? PromoCode = null
) : IQuery<BookingPriceBreakdown>;

/// <summary>
/// Input for flight segment pricing.
/// </summary>
public record FlightSegmentPriceInput(
    Guid FlightId,
    FlightClass CabinClass
);

/// <summary>
/// Detailed price breakdown for a booking.
/// </summary>
public record BookingPriceBreakdown(
    List<SegmentPriceBreakdown> Segments,
    decimal TotalBaseFare,
    decimal TotalTaxAmount,
    decimal ServiceFee,
    decimal DiscountAmount,
    string? DiscountDescription,
    decimal GrandTotal,
    string Currency
);

/// <summary>
/// Price breakdown for a single segment.
/// </summary>
public record SegmentPriceBreakdown(
    Guid FlightId,
    string FlightNumber,
    FlightClass CabinClass,
    decimal BaseFarePerAdult,
    decimal BaseFarePerChild,
    decimal BaseFarePerInfant,
    decimal TaxPerPax,
    decimal SegmentTotal
);

