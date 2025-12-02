using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Policies.Commands.CreateDynamicPricingRule;

/// <summary>
/// Command to create a new dynamic pricing rule.
/// </summary>
public record CreateDynamicPricingRuleCommand(
    string Name,
    string? Description,
    PricingRuleType RuleType,
    decimal AdjustmentPercentage,
    decimal? FixedAdjustment,
    string Currency,
    int Priority,
    // Day of Week parameters
    DayOfWeekFlag? ApplicableDays,
    // Seasonal parameters
    SeasonType? SeasonType,
    DateTime? SeasonStartDate,
    DateTime? SeasonEndDate,
    // Demand-based parameters
    decimal? MinBookingPercentage,
    decimal? MaxBookingPercentage,
    // Advance purchase parameters
    int? MinDaysBeforeDeparture,
    int? MaxDaysBeforeDeparture,
    // Time of day parameters
    int? StartHour,
    int? EndHour,
    // Scope restrictions
    Guid? AirlineId,
    Guid? DepartureAirportId,
    Guid? ArrivalAirportId,
    FlightClass? CabinClass
) : ICommand<Guid>;

