using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Airports.Queries.Common;

namespace FlightManagement.Application.Features.Airports.Queries.GetAllAirports;

/// <summary>
/// Query to get all airports with optional filtering.
/// </summary>
public record GetAllAirportsQuery(
    string? SearchTerm = null,
    string? Country = null,
    bool? IsActive = null
) : IQuery<List<AirportDto>>;

