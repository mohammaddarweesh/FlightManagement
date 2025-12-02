using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Seats.Queries.Common;

namespace FlightManagement.Application.Features.Seats.Queries.GetFlightSeatMap;

/// <summary>
/// Query to get the seat map for a specific flight.
/// </summary>
public record GetFlightSeatMapQuery(Guid FlightId) : IQuery<SeatMapDto>;

