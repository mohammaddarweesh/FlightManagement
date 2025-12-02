using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.SeasonalPricing.Queries.GetSeasonalPricingList;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.SeasonalPricing.Queries.GetSeasonalPricingById;

public class GetSeasonalPricingByIdQueryHandler : IQueryHandler<GetSeasonalPricingByIdQuery, SeasonalPricingDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetSeasonalPricingByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<SeasonalPricingDto>> Handle(
        GetSeasonalPricingByIdQuery request,
        CancellationToken cancellationToken)
    {
        var seasonalPricing = await _unitOfWork.Repository<Domain.Entities.SeasonalPricing>()
            .Query()
            .Include(sp => sp.Airline)
            .Include(sp => sp.DepartureAirport)
            .Include(sp => sp.ArrivalAirport)
            .FirstOrDefaultAsync(sp => sp.Id == request.Id, cancellationToken);

        if (seasonalPricing == null)
        {
            return Result<SeasonalPricingDto>.Failure($"Seasonal pricing with ID '{request.Id}' not found.");
        }

        var dto = new SeasonalPricingDto(
            seasonalPricing.Id,
            seasonalPricing.Name,
            seasonalPricing.Description,
            seasonalPricing.SeasonType,
            seasonalPricing.StartDate,
            seasonalPricing.EndDate,
            seasonalPricing.AdjustmentPercentage,
            seasonalPricing.AirlineId,
            seasonalPricing.Airline?.Name,
            seasonalPricing.DepartureAirportId,
            seasonalPricing.DepartureAirport?.IataCode,
            seasonalPricing.ArrivalAirportId,
            seasonalPricing.ArrivalAirport?.IataCode,
            seasonalPricing.CabinClass,
            seasonalPricing.Priority,
            seasonalPricing.IsActive
        );

        return Result<SeasonalPricingDto>.Success(dto);
    }
}

