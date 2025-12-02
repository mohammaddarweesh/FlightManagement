using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Aircraft.Commands.DeleteAircraft;

/// <summary>
/// Command to delete (soft delete) an aircraft.
/// </summary>
public record DeleteAircraftCommand(Guid Id) : ICommand;

