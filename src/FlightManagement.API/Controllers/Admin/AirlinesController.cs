using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Airlines.Commands.CreateAirline;
using FlightManagement.Application.Features.Airlines.Commands.DeleteAirline;
using FlightManagement.Application.Features.Airlines.Commands.UpdateAirline;
using FlightManagement.Application.Features.Airlines.Queries.GetAirlineById;
using FlightManagement.Application.Features.Airlines.Queries.GetAllAirlines;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Admin;

/// <summary>
/// Admin controller for managing airlines.
/// All endpoints require Admin role.
/// </summary>
[Route("api/admin/[controller]")]
[RequireAdmin]
public class AirlinesController : BaseApiController
{
    /// <summary>
    /// Get all airlines.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? searchTerm = null,
        [FromQuery] bool? isActive = null)
    {
        var result = await Mediator.Send(new GetAllAirlinesQuery(searchTerm, isActive));
        return HandleResult(result);
    }

    /// <summary>
    /// Get airline by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetAirlineByIdQuery(id));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Create a new airline.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAirlineCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Data });
    }

    /// <summary>
    /// Update an existing airline.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAirlineCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "ID mismatch" });

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Delete (soft delete) an airline.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteAirlineCommand(id));
        return HandleResult(result);
    }
}

