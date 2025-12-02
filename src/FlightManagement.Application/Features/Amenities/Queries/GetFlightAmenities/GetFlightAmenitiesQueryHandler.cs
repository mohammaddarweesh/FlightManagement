using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Amenities.Queries.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Amenities.Queries.GetFlightAmenities;

/// <summary>
/// Handler for getting amenities for a specific flight.
/// </summary>
public class GetFlightAmenitiesQueryHandler : IQueryHandler<GetFlightAmenitiesQuery, List<FlightAmenityDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetFlightAmenitiesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<FlightAmenityDto>>> Handle(GetFlightAmenitiesQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<FlightAmenity>();

        var flightAmenities = await repository.GetAllAsync(
            fa => fa.FlightId == request.FlightId &&
                  (request.CabinClass == null || fa.CabinClass == null || fa.CabinClass == request.CabinClass),
            cancellationToken,
            fa => fa.Amenity
        );

        var dtos = flightAmenities.Select(fa => new FlightAmenityDto(
            fa.Id,
            fa.Amenity.Code,
            fa.Amenity.Name,
            fa.Amenity.Description,
            fa.Amenity.Category,
            fa.CabinClass,
            fa.IsIncluded,
            fa.Price,
            fa.Currency
        )).ToList();

        return Result<List<FlightAmenityDto>>.Success(dtos);
    }
}

