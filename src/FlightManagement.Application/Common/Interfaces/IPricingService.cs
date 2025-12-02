using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Common.Interfaces;

/// <summary>
/// Service for calculating dynamic pricing including seasonal, demand-based, and day-of-week adjustments.
/// </summary>
public interface IPricingService
{
    /// <summary>
    /// Calculates the final price for a flight segment including all dynamic pricing adjustments.
    /// </summary>
    Task<PriceCalculationResult> CalculatePriceAsync(
        Guid flightId,
        FlightClass cabinClass,
        DateTime bookingDate,
        int passengerCount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all applicable pricing rules for a flight.
    /// </summary>
    Task<List<ApplicablePricingRule>> GetApplicablePricingRulesAsync(
        Guid flightId,
        FlightClass cabinClass,
        DateTime departureDate,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Result of price calculation with breakdown.
/// </summary>
public record PriceCalculationResult(
    decimal BasePrice,
    decimal AdjustedPrice,
    decimal TaxAmount,
    decimal TotalPrice,
    List<PriceAdjustment> Adjustments,
    string Currency
);

/// <summary>
/// Individual price adjustment applied.
/// </summary>
public record PriceAdjustment(
    string RuleName,
    PricingRuleType RuleType,
    decimal AdjustmentPercentage,
    decimal AdjustmentAmount
);

/// <summary>
/// Pricing rule that applies to a flight.
/// </summary>
public record ApplicablePricingRule(
    Guid RuleId,
    string RuleName,
    PricingRuleType RuleType,
    decimal AdjustmentPercentage,
    int Priority
);

