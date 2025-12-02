using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Flights.Commands.CancelFlight;

/// <summary>
/// Command to cancel a flight.
/// </summary>
public record CancelFlightCommand(
    Guid FlightId,
    string? CancellationReason = null
) : ICommand;

