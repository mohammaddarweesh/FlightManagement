using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Aircraft.Queries.Common;
using FlightManagement.Domain.Repositories;
using AircraftEntity = FlightManagement.Domain.Entities.Aircraft;

namespace FlightManagement.Application.Features.Aircraft.Queries.GetAircraftById;

/// <summary>
/// Handler for getting an aircraft by ID with cabin class details.
/// </summary>
public class GetAircraftByIdQueryHandler : IQueryHandler<GetAircraftByIdQuery, AircraftDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAircraftByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AircraftDto>> Handle(GetAircraftByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<AircraftEntity>();

        var aircraft = await repository.FirstOrDefaultAsync(
            a => a.Id == request.Id,
            cancellationToken,
            a => a.Airline,
            a => a.CabinClasses
        );

        if (aircraft == null)
        {
            return Result<AircraftDto>.Failure($"Aircraft with ID '{request.Id}' not found");
        }

        var dto = new AircraftDto(
            aircraft.Id,
            aircraft.AirlineId,
            aircraft.Airline.Name,
            aircraft.Model,
            aircraft.Manufacturer,
            aircraft.RegistrationNumber,
            aircraft.TotalSeats,
            aircraft.IsActive,
            aircraft.CreatedAt,
            aircraft.UpdatedAt,
            aircraft.CabinClasses.Select(c => new CabinClassDto(
                c.Id,
                c.CabinClass,
                c.SeatCount,
                c.RowStart,
                c.RowEnd,
                c.SeatLayout,
                c.BasePriceMultiplier
            )).OrderBy(c => c.CabinClass).ToList()
        );

        return Result<AircraftDto>.Success(dto);
    }
}

