using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Amenities.Queries.Common;

namespace FlightManagement.Application.Features.Amenities.Queries.GetAmenityById;

/// <summary>
/// Query to get an amenity by ID.
/// </summary>
public record GetAmenityByIdQuery(Guid Id) : IQuery<AmenityDto>;

