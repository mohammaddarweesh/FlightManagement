using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Airports.Commands.DeleteAirport;

/// <summary>
/// Command to delete (soft delete) an airport.
/// </summary>
public record DeleteAirportCommand(Guid Id) : ICommand;

