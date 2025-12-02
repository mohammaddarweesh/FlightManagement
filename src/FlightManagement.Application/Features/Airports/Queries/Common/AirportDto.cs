namespace FlightManagement.Application.Features.Airports.Queries.Common;

/// <summary>
/// Data transfer object for Airport entity.
/// </summary>
public record AirportDto(
    Guid Id,
    string IataCode,
    string IcaoCode,
    string Name,
    string City,
    string Country,
    string CountryCode,
    string Timezone,
    decimal Latitude,
    decimal Longitude,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

