using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Amenities.Commands.CreateAmenity;
using FlightManagement.Application.Features.Amenities.Commands.DeleteAmenity;
using FlightManagement.Application.Features.Amenities.Commands.UpdateAmenity;
using FlightManagement.Application.Features.Amenities.Queries.GetAllAmenities;
using FlightManagement.Application.Features.Amenities.Queries.GetAmenityById;
using FlightManagement.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Admin;

/// <summary>
/// Admin controller for managing amenities.
/// All endpoints require Admin role.
/// </summary>
[Route("api/admin/[controller]")]
[RequireAdmin]
public class AmenitiesController : BaseApiController
{
    /// <summary>
    /// Get all amenities with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] AmenityCategory? category = null,
        [FromQuery] bool? isActive = null)
    {
        var result = await Mediator.Send(new GetAllAmenitiesQuery(category, isActive));
        return HandleResult(result);
    }

    /// <summary>
    /// Get amenity by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetAmenityByIdQuery(id));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Create a new amenity.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAmenityCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Data });
    }

    /// <summary>
    /// Update an existing amenity.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAmenityCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "ID mismatch" });

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Delete (soft delete) an amenity.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteAmenityCommand(id));
        return HandleResult(result);
    }
}

