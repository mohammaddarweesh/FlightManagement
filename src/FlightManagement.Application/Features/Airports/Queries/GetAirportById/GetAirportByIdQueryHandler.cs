using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Airports.Queries.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Airports.Queries.GetAirportById;

/// <summary>
/// Handler for getting an airport by ID.
/// </summary>
public class GetAirportByIdQueryHandler : IQueryHandler<GetAirportByIdQuery, AirportDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAirportByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AirportDto>> Handle(GetAirportByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Airport>();
        var airport = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (airport == null)
        {
            return Result<AirportDto>.Failure($"Airport with ID '{request.Id}' not found");
        }

        var dto = new AirportDto(
            airport.Id,
            airport.IataCode,
            airport.IcaoCode,
            airport.Name,
            airport.City,
            airport.Country,
            airport.CountryCode,
            airport.Timezone,
            airport.Latitude,
            airport.Longitude,
            airport.IsActive,
            airport.CreatedAt,
            airport.UpdatedAt
        );

        return Result<AirportDto>.Success(dto);
    }
}

