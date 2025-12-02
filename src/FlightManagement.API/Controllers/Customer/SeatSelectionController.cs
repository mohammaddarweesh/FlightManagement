using System.Security.Claims;
using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Seats.Commands.ReleaseSeat;
using FlightManagement.Application.Features.Seats.Commands.ReserveSeat;
using FlightManagement.Application.Features.Seats.Queries.GetFlightSeatMap;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Customer;

/// <summary>
/// Customer controller for seat selection and reservation.
/// Accessible by both Customers and Admins.
/// </summary>
[Route("api/customer/seats")]
[RequireCustomer]
public class SeatSelectionController : BaseApiController
{
    /// <summary>
    /// Get the seat map for a flight.
    /// Returns all seats organized by cabin class with availability status.
    /// </summary>
    /// <param name="flightId">Flight ID</param>
    [HttpGet("flight/{flightId:guid}/map")]
    public async Task<IActionResult> GetSeatMap(Guid flightId)
    {
        var result = await Mediator.Send(new GetFlightSeatMapQuery(flightId));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Reserve a seat temporarily during booking process.
    /// The seat will be locked for 15 minutes by default.
    /// </summary>
    /// <param name="request">Seat reservation request</param>
    [HttpPost("reserve")]
    public async Task<IActionResult> ReserveSeat([FromBody] ReserveSeatRequest request)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();

        var command = new ReserveSeatCommand(
            request.FlightId,
            request.SeatId,
            userId.Value,
            request.LockDurationMinutes ?? 15
        );
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Release a previously reserved seat.
    /// </summary>
    /// <param name="flightSeatId">Flight seat ID to release</param>
    [HttpDelete("release/{flightSeatId:guid}")]
    public async Task<IActionResult> ReleaseSeat(Guid flightSeatId)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return Unauthorized();

        var command = new ReleaseSeatCommand(flightSeatId, userId.Value);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Gets the current user's ID from the JWT token.
    /// </summary>
    private Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(userIdClaim, out var userId))
            return userId;
        return null;
    }
}

/// <summary>
/// Request model for seat reservation.
/// </summary>
public record ReserveSeatRequest(
    Guid FlightId,
    Guid SeatId,
    int? LockDurationMinutes = null
);

