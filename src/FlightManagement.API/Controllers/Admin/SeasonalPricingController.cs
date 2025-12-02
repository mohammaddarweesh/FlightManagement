using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.SeasonalPricing.Commands.CreateSeasonalPricing;
using FlightManagement.Application.Features.SeasonalPricing.Commands.DeleteSeasonalPricing;
using FlightManagement.Application.Features.SeasonalPricing.Commands.UpdateSeasonalPricing;
using FlightManagement.Application.Features.SeasonalPricing.Queries.GetSeasonalPricingById;
using FlightManagement.Application.Features.SeasonalPricing.Queries.GetSeasonalPricingList;
using FlightManagement.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Admin;

[RequireAdmin]
[Route("api/admin/seasonal-pricing")]
public class SeasonalPricingController : BaseApiController
{
    /// <summary>
    /// Get all seasonal pricing periods.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] bool? isActive,
        [FromQuery] SeasonType? seasonType,
        [FromQuery] Guid? airlineId,
        [FromQuery] DateTime? date)
    {
        var result = await Mediator.Send(new GetSeasonalPricingListQuery(isActive, seasonType, airlineId, date));
        return HandleResult(result);
    }

    /// <summary>
    /// Get seasonal pricing by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetSeasonalPricingByIdQuery(id));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Create a new seasonal pricing period.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSeasonalPricingCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Data });
    }

    /// <summary>
    /// Update an existing seasonal pricing period.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSeasonalPricingCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Delete a seasonal pricing period.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteSeasonalPricingCommand(id));
        return HandleResult(result);
    }
}

