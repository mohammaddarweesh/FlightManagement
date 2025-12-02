using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Airlines.Queries.Common;

namespace FlightManagement.Application.Features.Airlines.Queries.GetAllAirlines;

/// <summary>
/// Query to get all airlines with optional filtering.
/// </summary>
public record GetAllAirlinesQuery(
    string? SearchTerm = null,
    bool? IsActive = null
) : IQuery<List<AirlineDto>>;

