using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Seats.Commands.ReleaseSeat;

/// <summary>
/// Command to release a previously reserved seat.
/// </summary>
public record ReleaseSeatCommand(
    Guid FlightSeatId,
    Guid UserId
) : ICommand;

