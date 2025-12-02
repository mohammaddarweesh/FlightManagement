using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Amenities.Queries.Common;

/// <summary>
/// Data transfer object for Amenity entity.
/// </summary>
public record AmenityDto(
    Guid Id,
    string Code,
    string Name,
    string Description,
    AmenityCategory Category,
    string? IconUrl,
    bool IsActive
);

/// <summary>
/// Flight amenity with pricing information.
/// </summary>
public record FlightAmenityDto(
    Guid Id,
    string Code,
    string Name,
    string Description,
    AmenityCategory Category,
    FlightClass? CabinClass,
    bool IsIncluded,
    decimal? Price,
    string? Currency
);

