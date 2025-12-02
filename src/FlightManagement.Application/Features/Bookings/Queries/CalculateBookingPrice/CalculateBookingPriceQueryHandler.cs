using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Bookings.Queries.CalculateBookingPrice;

/// <summary>
/// Handler for calculating booking price.
/// </summary>
public class CalculateBookingPriceQueryHandler : IQueryHandler<CalculateBookingPriceQuery, BookingPriceBreakdown>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CalculateBookingPriceQueryHandler> _logger;

    // Discount rates for different passenger types
    private const decimal ChildDiscountRate = 0.75m; // 25% discount
    private const decimal InfantDiscountRate = 0.10m; // 90% discount

    public CalculateBookingPriceQueryHandler(IUnitOfWork unitOfWork, ILogger<CalculateBookingPriceQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<BookingPriceBreakdown>> Handle(CalculateBookingPriceQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Calculating booking price for {SegmentCount} segments", request.Segments.Count);

        var flightRepo = _unitOfWork.Repository<Flight>();
        var pricingRepo = _unitOfWork.Repository<FlightPricing>();

        var segmentBreakdowns = new List<SegmentPriceBreakdown>();
        decimal totalBaseFare = 0;
        decimal totalTax = 0;
        string currency = "USD";

        foreach (var segmentInput in request.Segments)
        {
            var flight = await flightRepo.GetByIdAsync(segmentInput.FlightId, cancellationToken);
            if (flight == null)
                return Result<BookingPriceBreakdown>.Failure($"Flight with ID '{segmentInput.FlightId}' not found");

            var pricing = await pricingRepo.FirstOrDefaultAsync(
                p => p.FlightId == segmentInput.FlightId && p.CabinClass == segmentInput.CabinClass && p.IsActive,
                cancellationToken);

            if (pricing == null)
                return Result<BookingPriceBreakdown>.Failure(
                    $"No pricing found for flight '{flight.FlightNumber}' in {segmentInput.CabinClass} class");

            int totalPassengers = request.AdultCount + request.ChildCount + request.InfantCount;
            if (pricing.AvailableSeats < totalPassengers)
                return Result<BookingPriceBreakdown>.Failure(
                    $"Not enough seats available in {segmentInput.CabinClass} class on flight '{flight.FlightNumber}'");

            decimal baseFarePerAdult = pricing.CurrentPrice;
            decimal baseFarePerChild = pricing.CurrentPrice * ChildDiscountRate;
            decimal baseFarePerInfant = pricing.CurrentPrice * InfantDiscountRate;
            decimal taxPerPax = pricing.TaxAmount;

            decimal segmentBaseFare = 
                (baseFarePerAdult * request.AdultCount) +
                (baseFarePerChild * request.ChildCount) +
                (baseFarePerInfant * request.InfantCount);

            decimal segmentTax = taxPerPax * totalPassengers;
            decimal segmentTotal = segmentBaseFare + segmentTax;

            segmentBreakdowns.Add(new SegmentPriceBreakdown(
                flight.Id,
                flight.FlightNumber,
                segmentInput.CabinClass,
                baseFarePerAdult,
                baseFarePerChild,
                baseFarePerInfant,
                taxPerPax,
                segmentTotal
            ));

            totalBaseFare += segmentBaseFare;
            totalTax += segmentTax;
            currency = pricing.Currency;
        }

        // Calculate service fee
        decimal serviceFee = 10.00m; // Flat service fee

        // Apply promo code discount if provided
        decimal discountAmount = 0;
        string? discountDescription = null;
        if (!string.IsNullOrEmpty(request.PromoCode))
        {
            // Simple promo code logic - could be extended to use a promo code table
            var (discount, description) = ApplyPromoCode(request.PromoCode, totalBaseFare);
            discountAmount = discount;
            discountDescription = description;
        }

        decimal grandTotal = totalBaseFare + totalTax + serviceFee - discountAmount;

        return Result<BookingPriceBreakdown>.Success(new BookingPriceBreakdown(
            segmentBreakdowns,
            totalBaseFare,
            totalTax,
            serviceFee,
            discountAmount,
            discountDescription,
            grandTotal,
            currency
        ));
    }

    private static (decimal Discount, string? Description) ApplyPromoCode(string promoCode, decimal baseFare)
    {
        // Simple promo code examples - in production, this would query a promo codes table
        return promoCode.ToUpper() switch
        {
            "SAVE10" => (baseFare * 0.10m, "10% off base fare"),
            "SAVE20" => (baseFare * 0.20m, "20% off base fare"),
            "FLAT50" => (50.00m, "$50 off"),
            _ => (0, null)
        };
    }
}

