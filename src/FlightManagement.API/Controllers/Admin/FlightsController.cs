using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Flights.Commands.CancelFlight;
using FlightManagement.Application.Features.Flights.Commands.CreateFlight;
using FlightManagement.Application.Features.Flights.Commands.UpdateFlightStatus;
using FlightManagement.Application.Features.Flights.Queries.GetFlightById;
using FlightManagement.Application.Features.Flights.Queries.SearchFlights;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Admin;

/// <summary>
/// Admin controller for managing flights.
/// All endpoints require Admin role.
/// </summary>
[Route("api/admin/[controller]")]
[RequireAdmin]
public class FlightsController : BaseApiController
{
    /// <summary>
    /// Get all flights with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] SearchFlightsQuery query)
    {
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    /// <summary>
    /// Get flight by ID with full details.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetFlightByIdQuery(id));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Create a new flight with pricing and seat assignments.
    /// </summary>
    /// <remarks>
    /// This will:
    /// 1. Create the flight record
    /// 2. Create pricing for each cabin class
    /// 3. Create flight seat records for all aircraft seats
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateFlightCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Data });
    }

    /// <summary>
    /// Update flight status (e.g., Scheduled -> Boarding -> Departed).
    /// </summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateFlightStatusRequest request)
    {
        var command = new UpdateFlightStatusCommand(
            id,
            request.NewStatus,
            request.ActualDepartureTime,
            request.ActualArrivalTime
        );
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Cancel a flight.
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelFlightRequest? request = null)
    {
        var command = new CancelFlightCommand(id, request?.Reason);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}

/// <summary>
/// Request model for updating flight status.
/// </summary>
public record UpdateFlightStatusRequest(
    FlightManagement.Domain.Enums.FlightStatus NewStatus,
    DateTime? ActualDepartureTime = null,
    DateTime? ActualArrivalTime = null
);

/// <summary>
/// Request model for cancelling a flight.
/// </summary>
public record CancelFlightRequest(string? Reason = null);

