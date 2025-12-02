using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.SeasonalPricing.Queries.GetSeasonalPricingList;

public class GetSeasonalPricingListQueryHandler : IQueryHandler<GetSeasonalPricingListQuery, IEnumerable<SeasonalPricingDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSeasonalPricingListQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<SeasonalPricingDto>>> Handle(
        GetSeasonalPricingListQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<Domain.Entities.SeasonalPricing>().Query();

        if (request.IsActive.HasValue)
            query = query.Where(sp => sp.IsActive == request.IsActive.Value);

        if (request.SeasonType.HasValue)
            query = query.Where(sp => sp.SeasonType == request.SeasonType.Value);

        if (request.AirlineId.HasValue)
            query = query.Where(sp => sp.AirlineId == request.AirlineId.Value || sp.AirlineId == null);

        if (request.Date.HasValue)
            query = query.Where(sp => sp.StartDate <= request.Date.Value && sp.EndDate >= request.Date.Value);

        var seasonalPricings = await query
            .Include(sp => sp.Airline)
            .Include(sp => sp.DepartureAirport)
            .Include(sp => sp.ArrivalAirport)
            .OrderByDescending(sp => sp.Priority)
            .ThenBy(sp => sp.StartDate)
            .ToListAsync(cancellationToken);

        var dtos = seasonalPricings.Select(sp => new SeasonalPricingDto(
            sp.Id,
            sp.Name,
            sp.Description,
            sp.SeasonType,
            sp.StartDate,
            sp.EndDate,
            sp.AdjustmentPercentage,
            sp.AirlineId,
            sp.Airline?.Name,
            sp.DepartureAirportId,
            sp.DepartureAirport?.IataCode,
            sp.ArrivalAirportId,
            sp.ArrivalAirport?.IataCode,
            sp.CabinClass,
            sp.Priority,
            sp.IsActive
        ));

        return Result<IEnumerable<SeasonalPricingDto>>.Success(dtos);
    }
}

