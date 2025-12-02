using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Flights.Commands.UpdateFlightStatus;

/// <summary>
/// Command to update a flight's status.
/// </summary>
public record UpdateFlightStatusCommand(
    Guid FlightId,
    FlightStatus NewStatus,
    DateTime? ActualDepartureTime = null,
    DateTime? ActualArrivalTime = null
) : ICommand;

