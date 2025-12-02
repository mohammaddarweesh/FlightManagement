using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Airports.Commands.CreateAirport;
using FlightManagement.Application.Features.Airports.Commands.DeleteAirport;
using FlightManagement.Application.Features.Airports.Commands.UpdateAirport;
using FlightManagement.Application.Features.Airports.Queries.GetAirportById;
using FlightManagement.Application.Features.Airports.Queries.GetAllAirports;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Admin;

/// <summary>
/// Admin controller for managing airports.
/// All endpoints require Admin role.
/// </summary>
[Route("api/admin/[controller]")]
[RequireAdmin]
public class AirportsController : BaseApiController
{
    /// <summary>
    /// Get all airports.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? searchTerm = null,
        [FromQuery] string? country = null,
        [FromQuery] bool? isActive = null)
    {
        var result = await Mediator.Send(new GetAllAirportsQuery(searchTerm, country, isActive));
        return HandleResult(result);
    }

    /// <summary>
    /// Get airport by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetAirportByIdQuery(id));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Create a new airport.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAirportCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Data });
    }

    /// <summary>
    /// Update an existing airport.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAirportCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "ID mismatch" });

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Delete (soft delete) an airport.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteAirportCommand(id));
        return HandleResult(result);
    }
}

