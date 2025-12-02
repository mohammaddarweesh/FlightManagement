using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Airlines.Queries.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Airlines.Queries.GetAirlineById;

/// <summary>
/// Handler for getting an airline by ID.
/// </summary>
public class GetAirlineByIdQueryHandler : IQueryHandler<GetAirlineByIdQuery, AirlineDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAirlineByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AirlineDto>> Handle(GetAirlineByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Airline>();
        var airline = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (airline == null)
        {
            return Result<AirlineDto>.Failure($"Airline with ID '{request.Id}' not found");
        }

        var dto = new AirlineDto(
            airline.Id,
            airline.IataCode,
            airline.IcaoCode,
            airline.Name,
            airline.Country,
            airline.LogoUrl,
            airline.IsActive,
            airline.CreatedAt,
            airline.UpdatedAt
        );

        return Result<AirlineDto>.Success(dto);
    }
}

