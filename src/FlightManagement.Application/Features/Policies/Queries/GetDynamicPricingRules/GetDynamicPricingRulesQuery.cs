using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Policies.Queries.GetDynamicPricingRules;

public record GetDynamicPricingRulesQuery(
    PricingRuleType? RuleType = null,
    bool? IsActive = null,
    Guid? AirlineId = null,
    int Page = 1,
    int PageSize = 20
) : IQuery<DynamicPricingRuleListResult>;

public record DynamicPricingRuleListResult(
    List<DynamicPricingRuleDto> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

public record DynamicPricingRuleDto(
    Guid Id,
    string Name,
    string? Description,
    PricingRuleType RuleType,
    decimal AdjustmentPercentage,
    decimal? FixedAdjustment,
    string Currency,
    int Priority,
    DayOfWeekFlag? ApplicableDays,
    SeasonType? SeasonType,
    DateTime? SeasonStartDate,
    DateTime? SeasonEndDate,
    decimal? MinBookingPercentage,
    decimal? MaxBookingPercentage,
    int? MinDaysBeforeDeparture,
    int? MaxDaysBeforeDeparture,
    int? StartHour,
    int? EndHour,
    Guid? AirlineId,
    string? AirlineName,
    FlightClass? CabinClass,
    bool IsActive
);

