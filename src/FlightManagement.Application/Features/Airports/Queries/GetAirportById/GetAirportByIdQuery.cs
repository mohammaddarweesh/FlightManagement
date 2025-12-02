using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Airports.Queries.Common;

namespace FlightManagement.Application.Features.Airports.Queries.GetAirportById;

/// <summary>
/// Query to get an airport by its ID.
/// </summary>
public record GetAirportByIdQuery(Guid Id) : IQuery<AirportDto>;

