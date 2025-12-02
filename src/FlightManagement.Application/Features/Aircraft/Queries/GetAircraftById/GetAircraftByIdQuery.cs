using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Aircraft.Queries.Common;

namespace FlightManagement.Application.Features.Aircraft.Queries.GetAircraftById;

/// <summary>
/// Query to get an aircraft by ID with cabin class details.
/// </summary>
public record GetAircraftByIdQuery(Guid Id) : IQuery<AircraftDto>;

