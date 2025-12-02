using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Airports.Queries.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Airports.Queries.GetAllAirports;

/// <summary>
/// Handler for getting all airports with optional filtering.
/// </summary>
public class GetAllAirportsQueryHandler : IQueryHandler<GetAllAirportsQuery, List<AirportDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllAirportsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<AirportDto>>> Handle(GetAllAirportsQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Airport>();

        // Build filter expression
        var airports = await repository.GetAllAsync(
            a => (request.IsActive == null || a.IsActive == request.IsActive) &&
                 (string.IsNullOrEmpty(request.Country) || a.Country.ToLower().Contains(request.Country.ToLower())) &&
                 (string.IsNullOrEmpty(request.SearchTerm) || 
                  a.Name.ToLower().Contains(request.SearchTerm.ToLower()) ||
                  a.City.ToLower().Contains(request.SearchTerm.ToLower()) ||
                  a.IataCode.ToLower().Contains(request.SearchTerm.ToLower()) ||
                  a.IcaoCode.ToLower().Contains(request.SearchTerm.ToLower())),
            cancellationToken
        );

        var dtos = airports.Select(a => new AirportDto(
            a.Id,
            a.IataCode,
            a.IcaoCode,
            a.Name,
            a.City,
            a.Country,
            a.CountryCode,
            a.Timezone,
            a.Latitude,
            a.Longitude,
            a.IsActive,
            a.CreatedAt,
            a.UpdatedAt
        )).ToList();

        return Result<List<AirportDto>>.Success(dtos);
    }
}

