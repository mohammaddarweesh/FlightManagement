using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Policies.Commands.CreateBlackoutDate;
using FlightManagement.Application.Features.Policies.Commands.CreateBookingPolicy;
using FlightManagement.Application.Features.Policies.Commands.CreateDynamicPricingRule;
using FlightManagement.Application.Features.Policies.Commands.CreateOverbookingPolicy;
using FlightManagement.Application.Features.Policies.Queries.GetBlackoutDates;
using FlightManagement.Application.Features.Policies.Queries.GetBookingPolicies;
using FlightManagement.Application.Features.Policies.Queries.GetDynamicPricingRules;
using FlightManagement.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Admin;

/// <summary>
/// Admin controller for managing booking policies, blackout dates, and pricing rules.
/// All endpoints require Admin role.
/// </summary>
[Route("api/admin/[controller]")]
[RequireAdmin]
public class PoliciesController : BaseApiController
{
    #region Booking Policies

    /// <summary>
    /// Get all booking policies with optional filtering.
    /// </summary>
    [HttpGet("booking")]
    public async Task<IActionResult> GetBookingPolicies(
        [FromQuery] PolicyType? type = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] Guid? airlineId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await Mediator.Send(new GetBookingPoliciesQuery(type, isActive, airlineId, page, pageSize));
        return HandleResult(result);
    }

    /// <summary>
    /// Create a new booking policy.
    /// </summary>
    [HttpPost("booking")]
    public async Task<IActionResult> CreateBookingPolicy([FromBody] CreateBookingPolicyCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, "GetBookingPolicies", null);
    }

    #endregion

    #region Blackout Dates

    /// <summary>
    /// Get all blackout dates with optional filtering.
    /// </summary>
    [HttpGet("blackout-dates")]
    public async Task<IActionResult> GetBlackoutDates(
        [FromQuery] bool? isActive = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] Guid? airlineId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await Mediator.Send(new GetBlackoutDatesQuery(isActive, fromDate, toDate, airlineId, page, pageSize));
        return HandleResult(result);
    }

    /// <summary>
    /// Create a new blackout date period.
    /// </summary>
    [HttpPost("blackout-dates")]
    public async Task<IActionResult> CreateBlackoutDate([FromBody] CreateBlackoutDateCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, "GetBlackoutDates", null);
    }

    #endregion

    #region Dynamic Pricing Rules

    /// <summary>
    /// Get all dynamic pricing rules with optional filtering.
    /// </summary>
    [HttpGet("pricing-rules")]
    public async Task<IActionResult> GetDynamicPricingRules(
        [FromQuery] PricingRuleType? ruleType = null,
        [FromQuery] bool? isActive = null,
        [FromQuery] Guid? airlineId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var result = await Mediator.Send(new GetDynamicPricingRulesQuery(ruleType, isActive, airlineId, page, pageSize));
        return HandleResult(result);
    }

    /// <summary>
    /// Create a new dynamic pricing rule.
    /// </summary>
    [HttpPost("pricing-rules")]
    public async Task<IActionResult> CreateDynamicPricingRule([FromBody] CreateDynamicPricingRuleCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, "GetDynamicPricingRules", null);
    }

    #endregion

    #region Overbooking Policies

    /// <summary>
    /// Create a new overbooking policy.
    /// </summary>
    [HttpPost("overbooking")]
    public async Task<IActionResult> CreateOverbookingPolicy([FromBody] CreateOverbookingPolicyCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, "GetBookingPolicies", null);
    }

    #endregion
}

