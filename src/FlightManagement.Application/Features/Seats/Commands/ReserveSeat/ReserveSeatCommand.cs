using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Seats.Commands.ReserveSeat;

/// <summary>
/// Command to temporarily reserve a seat during booking process.
/// The seat will be locked for a specified duration.
/// </summary>
public record ReserveSeatCommand(
    Guid FlightId,
    Guid SeatId,
    Guid UserId,
    int LockDurationMinutes = 15
) : ICommand<Guid>;

