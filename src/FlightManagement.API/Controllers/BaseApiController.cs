using FlightManagement.Application.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers;

/// <summary>
/// Base controller for all API controllers with common functionality
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    private IMediator? _mediator;

    /// <summary>
    /// Gets the MediatR instance from DI
    /// </summary>
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

    /// <summary>
    /// Handles the result and returns appropriate HTTP response
    /// </summary>
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result.Data);

        return BadRequest(new { errors = result.Errors });
    }

    /// <summary>
    /// Handles the result without data and returns appropriate HTTP response
    /// </summary>
    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
            return Ok();

        return BadRequest(new { errors = result.Errors });
    }

    /// <summary>
    /// Handles the result for creation and returns 201 Created
    /// </summary>
    protected IActionResult HandleCreatedResult<T>(Result<T> result, string actionName, object? routeValues = null)
    {
        if (result.IsSuccess)
            return CreatedAtAction(actionName, routeValues, result.Data);

        return BadRequest(new { errors = result.Errors });
    }

    /// <summary>
    /// Handles the result for not found scenarios
    /// </summary>
    protected IActionResult HandleNotFoundResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result.Data);

        return NotFound(new { errors = result.Errors });
    }
}

