namespace FlightManagement.Application.Features.Airlines.Queries.Common;

/// <summary>
/// Data transfer object for Airline entity.
/// </summary>
public record AirlineDto(
    Guid Id,
    string IataCode,
    string IcaoCode,
    string Name,
    string Country,
    string? LogoUrl,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

