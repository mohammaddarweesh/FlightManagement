using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Airlines.Queries.Common;

namespace FlightManagement.Application.Features.Airlines.Queries.GetAirlineById;

/// <summary>
/// Query to get an airline by ID.
/// </summary>
public record GetAirlineByIdQuery(Guid Id) : IQuery<AirlineDto>;

