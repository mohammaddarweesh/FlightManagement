using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Amenities.Queries.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Amenities.Queries.GetAllAmenities;

/// <summary>
/// Handler for getting all amenities.
/// </summary>
public class GetAllAmenitiesQueryHandler : IQueryHandler<GetAllAmenitiesQuery, List<AmenityDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllAmenitiesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<AmenityDto>>> Handle(GetAllAmenitiesQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Amenity>();

        var amenities = await repository.GetAllAsync(
            a => (request.IsActive == null || a.IsActive == request.IsActive) &&
                 (request.Category == null || a.Category == request.Category),
            cancellationToken
        );

        var dtos = amenities.Select(a => new AmenityDto(
            a.Id,
            a.Code,
            a.Name,
            a.Description,
            a.Category,
            a.IconUrl,
            a.IsActive
        )).ToList();

        return Result<List<AmenityDto>>.Success(dtos);
    }
}

