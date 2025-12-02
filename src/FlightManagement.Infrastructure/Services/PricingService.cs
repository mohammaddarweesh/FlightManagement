using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Services;

public class PricingService : IPricingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PricingService> _logger;

    public PricingService(IUnitOfWork unitOfWork, ILogger<PricingService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PriceCalculationResult> CalculatePriceAsync(
        Guid flightId,
        FlightClass cabinClass,
        DateTime bookingDate,
        int passengerCount,
        CancellationToken cancellationToken = default)
    {
        // Get flight with pricing
        var flightRepo = _unitOfWork.Repository<Flight>();
        var flight = await flightRepo.Query()
            .Include(f => f.FlightPricings)
            .Include(f => f.Airline)
            .FirstOrDefaultAsync(f => f.Id == flightId, cancellationToken);

        if (flight == null)
            throw new InvalidOperationException($"Flight {flightId} not found");

        var pricing = flight.FlightPricings.FirstOrDefault(p => p.CabinClass == cabinClass);
        if (pricing == null)
            throw new InvalidOperationException($"No pricing found for cabin class {cabinClass}");

        decimal basePrice = pricing.CurrentPrice;
        decimal adjustedPrice = basePrice;
        var adjustments = new List<PriceAdjustment>();

        // Get applicable pricing rules
        var rules = await GetApplicablePricingRulesAsync(flightId, cabinClass, flight.ScheduledDepartureTime, cancellationToken);

        foreach (var rule in rules.OrderByDescending(r => r.Priority))
        {
            var adjustmentAmount = basePrice * (rule.AdjustmentPercentage / 100);
            adjustedPrice += adjustmentAmount;
            adjustments.Add(new PriceAdjustment(rule.RuleName, rule.RuleType, rule.AdjustmentPercentage, adjustmentAmount));
        }

        // Apply seasonal pricing
        var seasonalAdjustment = await GetSeasonalAdjustmentAsync(flight, cabinClass, cancellationToken);
        if (seasonalAdjustment.HasValue)
        {
            var seasonalAmount = basePrice * (seasonalAdjustment.Value / 100);
            adjustedPrice += seasonalAmount;
            adjustments.Add(new PriceAdjustment("Seasonal Pricing", PricingRuleType.Seasonal, seasonalAdjustment.Value, seasonalAmount));
        }

        // Apply demand-based pricing
        var demandAdjustment = CalculateDemandAdjustment(pricing);
        if (demandAdjustment != 0)
        {
            var demandAmount = basePrice * (demandAdjustment / 100);
            adjustedPrice += demandAmount;
            adjustments.Add(new PriceAdjustment("Demand-Based Pricing", PricingRuleType.DemandBased, demandAdjustment, demandAmount));
        }

        // Ensure price doesn't go below a minimum threshold
        adjustedPrice = Math.Max(adjustedPrice, basePrice * 0.5m);

        decimal taxAmount = pricing.TaxAmount;
        decimal totalPrice = (adjustedPrice + taxAmount) * passengerCount;

        return new PriceCalculationResult(
            basePrice,
            adjustedPrice,
            taxAmount,
            totalPrice,
            adjustments,
            pricing.Currency
        );
    }

    public async Task<List<ApplicablePricingRule>> GetApplicablePricingRulesAsync(
        Guid flightId,
        FlightClass cabinClass,
        DateTime departureDate,
        CancellationToken cancellationToken = default)
    {
        var flight = await _unitOfWork.Repository<Flight>()
            .Query()
            .Include(f => f.Airline)
            .FirstOrDefaultAsync(f => f.Id == flightId, cancellationToken);

        if (flight == null) return new List<ApplicablePricingRule>();

        var ruleRepo = _unitOfWork.Repository<DynamicPricingRule>();
        var rules = await ruleRepo.Query()
            .Where(r => r.IsActive)
            .Where(r => r.AirlineId == null || r.AirlineId == flight.AirlineId)
            .Where(r => r.DepartureAirportId == null || r.DepartureAirportId == flight.DepartureAirportId)
            .Where(r => r.ArrivalAirportId == null || r.ArrivalAirportId == flight.ArrivalAirportId)
            .Where(r => r.CabinClass == null || r.CabinClass == cabinClass)
            .ToListAsync(cancellationToken);

        var applicableRules = new List<ApplicablePricingRule>();
        var dayOfWeek = (DayOfWeekFlag)(1 << (int)departureDate.DayOfWeek);

        foreach (var rule in rules)
        {
            bool applies = rule.RuleType switch
            {
                PricingRuleType.DayOfWeek => rule.ApplicableDays.HasValue && rule.ApplicableDays.Value.HasFlag(dayOfWeek),
                PricingRuleType.AdvancePurchase => IsAdvancePurchaseApplicable(rule, departureDate),
                PricingRuleType.LastMinute => IsLastMinuteApplicable(rule, departureDate),
                PricingRuleType.TimeOfDay => IsTimeOfDayApplicable(rule, departureDate),
                _ => true
            };

            if (applies)
            {
                applicableRules.Add(new ApplicablePricingRule(
                    rule.Id, rule.Name, rule.RuleType, rule.AdjustmentPercentage, rule.Priority));
            }
        }

        return applicableRules;
    }

    private async Task<decimal?> GetSeasonalAdjustmentAsync(Flight flight, FlightClass cabinClass, CancellationToken ct)
    {
        var seasonalRepo = _unitOfWork.Repository<SeasonalPricing>();
        var seasonal = await seasonalRepo.Query()
            .Where(s => s.IsActive)
            .Where(s => s.StartDate <= flight.ScheduledDepartureTime && s.EndDate >= flight.ScheduledDepartureTime)
            .Where(s => s.AirlineId == null || s.AirlineId == flight.AirlineId)
            .Where(s => s.CabinClass == null || s.CabinClass == cabinClass)
            .OrderByDescending(s => s.Priority)
            .FirstOrDefaultAsync(ct);

        return seasonal?.AdjustmentPercentage;
    }

    private decimal CalculateDemandAdjustment(FlightPricing pricing)
    {
        // Dynamic demand-based pricing based on booking percentage
        var bookingPercentage = pricing.BookingPercentage;
        return bookingPercentage switch
        {
            >= 90 => 25,  // 25% increase when nearly sold out
            >= 80 => 15,  // 15% increase when 80%+ booked
            >= 70 => 10,  // 10% increase when 70%+ booked
            >= 50 => 5,   // 5% increase when 50%+ booked
            < 20 => -10,  // 10% discount when less than 20% booked
            _ => 0
        };
    }

    private bool IsAdvancePurchaseApplicable(DynamicPricingRule rule, DateTime departureDate)
    {
        var daysBeforeDeparture = (departureDate.Date - DateTime.UtcNow.Date).Days;
        return (!rule.MinDaysBeforeDeparture.HasValue || daysBeforeDeparture >= rule.MinDaysBeforeDeparture.Value) &&
               (!rule.MaxDaysBeforeDeparture.HasValue || daysBeforeDeparture <= rule.MaxDaysBeforeDeparture.Value);
    }

    private bool IsLastMinuteApplicable(DynamicPricingRule rule, DateTime departureDate)
    {
        var daysBeforeDeparture = (departureDate.Date - DateTime.UtcNow.Date).Days;
        return daysBeforeDeparture <= (rule.MaxDaysBeforeDeparture ?? 3);
    }

    private bool IsTimeOfDayApplicable(DynamicPricingRule rule, DateTime departureDate)
    {
        var hour = departureDate.Hour;
        return (!rule.StartHour.HasValue || hour >= rule.StartHour.Value) &&
               (!rule.EndHour.HasValue || hour <= rule.EndHour.Value);
    }
}

