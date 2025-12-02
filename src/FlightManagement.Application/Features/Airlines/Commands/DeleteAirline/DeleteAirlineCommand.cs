using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Airlines.Commands.DeleteAirline;

/// <summary>
/// Command to delete (soft delete) an airline.
/// </summary>
public record DeleteAirlineCommand(Guid Id) : ICommand;

