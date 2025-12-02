using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Amenities.Queries.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Amenities.Queries.GetAmenityById;

/// <summary>
/// Handler for getting an amenity by ID.
/// </summary>
public class GetAmenityByIdQueryHandler : IQueryHandler<GetAmenityByIdQuery, AmenityDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAmenityByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<AmenityDto>> Handle(GetAmenityByIdQuery request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Amenity>();
        var amenity = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (amenity == null)
        {
            return Result<AmenityDto>.Failure($"Amenity with ID '{request.Id}' not found");
        }

        var dto = new AmenityDto(
            amenity.Id,
            amenity.Code,
            amenity.Name,
            amenity.Description,
            amenity.Category,
            amenity.IconUrl,
            amenity.IsActive
        );

        return Result<AmenityDto>.Success(dto);
    }
}

