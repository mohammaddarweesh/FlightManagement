using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Airlines.Queries.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Airlines.Queries.GetAllAirlines;

/// <summary>
/// Handler for getting all airlines with optional filtering.
/// </summary>
public class GetAllAirlinesQueryHandler : IQueryHandler<GetAllAirlinesQuery, List<AirlineDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllAirlinesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<AirlineDto>>> Handle(GetAllAirlinesQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Airline>();

        var airlines = await repository.GetAllAsync(
            a => (request.IsActive == null || a.IsActive == request.IsActive) &&
                 (string.IsNullOrEmpty(request.SearchTerm) || 
                  a.Name.ToLower().Contains(request.SearchTerm.ToLower()) ||
                  a.IataCode.ToLower().Contains(request.SearchTerm.ToLower()) ||
                  a.IcaoCode.ToLower().Contains(request.SearchTerm.ToLower())),
            cancellationToken
        );

        var dtos = airlines.Select(a => new AirlineDto(
            a.Id,
            a.IataCode,
            a.IcaoCode,
            a.Name,
            a.Country,
            a.LogoUrl,
            a.IsActive,
            a.CreatedAt,
            a.UpdatedAt
        )).ToList();

        return Result<List<AirlineDto>>.Success(dtos);
    }
}

