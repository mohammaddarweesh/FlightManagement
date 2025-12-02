using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Aircraft.Queries.Common;

namespace FlightManagement.Application.Features.Aircraft.Queries.GetAllAircraft;

/// <summary>
/// Query to get all aircraft with optional filtering.
/// </summary>
public record GetAllAircraftQuery(
    Guid? AirlineId = null,
    string? SearchTerm = null,
    bool? IsActive = null
) : IQuery<List<AircraftSimpleDto>>;

