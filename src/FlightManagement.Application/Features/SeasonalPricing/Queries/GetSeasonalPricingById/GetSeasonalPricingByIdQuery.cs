using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.SeasonalPricing.Queries.GetSeasonalPricingList;

namespace FlightManagement.Application.Features.SeasonalPricing.Queries.GetSeasonalPricingById;

/// <summary>
/// Query to get a seasonal pricing by ID.
/// </summary>
public record GetSeasonalPricingByIdQuery(Guid Id) : IQuery<SeasonalPricingDto>;

