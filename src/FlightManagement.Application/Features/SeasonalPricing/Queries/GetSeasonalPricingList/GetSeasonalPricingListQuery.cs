using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.SeasonalPricing.Queries.GetSeasonalPricingList;

/// <summary>
/// Query to get seasonal pricing periods.
/// </summary>
public record GetSeasonalPricingListQuery(
    bool? IsActive = null,
    SeasonType? SeasonType = null,
    Guid? AirlineId = null,
    DateTime? Date = null
) : IQuery<IEnumerable<SeasonalPricingDto>>;

public record SeasonalPricingDto(
    Guid Id,
    string Name,
    string Description,
    SeasonType SeasonType,
    DateTime StartDate,
    DateTime EndDate,
    decimal AdjustmentPercentage,
    Guid? AirlineId,
    string? AirlineName,
    Guid? DepartureAirportId,
    string? DepartureAirportCode,
    Guid? ArrivalAirportId,
    string? ArrivalAirportCode,
    FlightClass? CabinClass,
    int Priority,
    bool IsActive
);

