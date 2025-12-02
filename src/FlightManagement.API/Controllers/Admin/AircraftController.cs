using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Aircraft.Commands.CreateAircraft;
using FlightManagement.Application.Features.Aircraft.Commands.DeleteAircraft;
using FlightManagement.Application.Features.Aircraft.Commands.UpdateAircraft;
using FlightManagement.Application.Features.Aircraft.Queries.GetAircraftById;
using FlightManagement.Application.Features.Aircraft.Queries.GetAllAircraft;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Admin;

/// <summary>
/// Admin controller for managing aircraft.
/// All endpoints require Admin role.
/// </summary>
[Route("api/admin/[controller]")]
[RequireAdmin]
public class AircraftController : BaseApiController
{
    /// <summary>
    /// Get all aircraft with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? airlineId = null,
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isActive = null)
    {
        var result = await Mediator.Send(new GetAllAircraftQuery(airlineId, searchTerm, isActive));
        return HandleResult(result);
    }

    /// <summary>
    /// Get aircraft by ID with cabin class details.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetAircraftByIdQuery(id));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Create a new aircraft with cabin class configuration.
    /// </summary>
    /// <remarks>
    /// This will:
    /// 1. Create the aircraft record
    /// 2. Create cabin class configurations
    /// 3. Generate all seats based on cabin layouts
    /// </remarks>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAircraftCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Data });
    }

    /// <summary>
    /// Update an existing aircraft.
    /// Note: Cabin classes and seats cannot be modified after creation.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAircraftCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "ID mismatch" });

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Delete (soft delete) an aircraft.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteAircraftCommand(id));
        return HandleResult(result);
    }
}

