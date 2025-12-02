using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Flights.Queries.Common;

namespace FlightManagement.Application.Features.Flights.Queries.GetFlightById;

/// <summary>
/// Query to get a flight by ID with all details.
/// </summary>
public record GetFlightByIdQuery(Guid Id) : IQuery<FlightDto>;

