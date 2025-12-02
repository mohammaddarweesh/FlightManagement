using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Promotions.Commands.ActivatePromotion;
using FlightManagement.Application.Features.Promotions.Commands.CreatePromotion;
using FlightManagement.Application.Features.Promotions.Commands.DeactivatePromotion;
using FlightManagement.Application.Features.Promotions.Commands.UpdatePromotion;
using FlightManagement.Application.Features.Promotions.Queries.GetPromotionById;
using FlightManagement.Application.Features.Promotions.Queries.GetPromotions;
using FlightManagement.Application.Features.Promotions.Queries.GetPromotionUsage;
using FlightManagement.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Admin;

/// <summary>
/// Admin controller for managing promotions and discount codes.
/// All endpoints require Admin role.
/// </summary>
[Route("api/admin/[controller]")]
[RequireAdmin]
public class PromotionsController : BaseApiController
{
    /// <summary>
    /// Get all promotions with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] PromotionStatus? status = null,
        [FromQuery] PromotionType? type = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await Mediator.Send(new GetPromotionsQuery(status, type, isActive, page, pageSize));
        return HandleResult(result);
    }

    /// <summary>
    /// Get promotion by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetPromotionByIdQuery(id));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Get promotion usage history.
    /// </summary>
    [HttpGet("{id:guid}/usage")]
    public async Task<IActionResult> GetUsage(
        Guid id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await Mediator.Send(new GetPromotionUsageQuery(id, page, pageSize));
        return HandleResult(result);
    }

    /// <summary>
    /// Create a new promotion.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePromotionCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Data });
    }

    /// <summary>
    /// Update an existing promotion.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePromotionCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "ID mismatch" });

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Activate a promotion.
    /// </summary>
    [HttpPost("{id:guid}/activate")]
    public async Task<IActionResult> Activate(Guid id)
    {
        var result = await Mediator.Send(new ActivatePromotionCommand(id));
        return HandleResult(result);
    }

    /// <summary>
    /// Deactivate a promotion.
    /// </summary>
    [HttpPost("{id:guid}/deactivate")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        var result = await Mediator.Send(new DeactivatePromotionCommand(id));
        return HandleResult(result);
    }
}

