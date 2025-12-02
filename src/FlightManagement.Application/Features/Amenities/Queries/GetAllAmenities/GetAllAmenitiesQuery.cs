using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Amenities.Queries.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Amenities.Queries.GetAllAmenities;

/// <summary>
/// Query to get all amenities with optional filtering.
/// </summary>
public record GetAllAmenitiesQuery(
    AmenityCategory? Category = null,
    bool? IsActive = null
) : IQuery<List<AmenityDto>>;

