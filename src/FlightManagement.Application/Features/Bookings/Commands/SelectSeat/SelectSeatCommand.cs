using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Bookings.Commands.SelectSeat;

/// <summary>
/// Command to select a seat for a passenger on a specific segment.
/// </summary>
public record SelectSeatCommand(
    Guid BookingId,
    Guid PassengerId,
    Guid BookingSegmentId,
    Guid FlightSeatId
) : ICommand<SelectSeatResult>;

/// <summary>
/// Result of seat selection.
/// </summary>
public record SelectSeatResult(
    Guid PassengerSeatId,
    string SeatNumber,
    decimal SeatFee
);

