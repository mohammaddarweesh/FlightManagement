using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Aircraft.Queries.Common;
using FlightManagement.Domain.Repositories;
using AircraftEntity = FlightManagement.Domain.Entities.Aircraft;

namespace FlightManagement.Application.Features.Aircraft.Queries.GetAllAircraft;

/// <summary>
/// Handler for getting all aircraft with optional filtering.
/// </summary>
public class GetAllAircraftQueryHandler : IQueryHandler<GetAllAircraftQuery, List<AircraftSimpleDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllAircraftQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<AircraftSimpleDto>>> Handle(GetAllAircraftQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<AircraftEntity>();

        var aircraft = await repository.GetAllAsync(
            a => (request.IsActive == null || a.IsActive == request.IsActive) &&
                 (request.AirlineId == null || a.AirlineId == request.AirlineId) &&
                 (string.IsNullOrEmpty(request.SearchTerm) ||
                  a.Model.ToLower().Contains(request.SearchTerm.ToLower()) ||
                  a.Manufacturer.ToLower().Contains(request.SearchTerm.ToLower()) ||
                  a.RegistrationNumber.ToLower().Contains(request.SearchTerm.ToLower())),
            cancellationToken,
            a => a.Airline
        );

        var dtos = aircraft.Select(a => new AircraftSimpleDto(
            a.Id,
            a.Airline.Name,
            a.Model,
            a.Manufacturer,
            a.RegistrationNumber,
            a.TotalSeats,
            a.IsActive
        )).ToList();

        return Result<List<AircraftSimpleDto>>.Success(dtos);
    }
}

