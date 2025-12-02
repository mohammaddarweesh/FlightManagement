using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.CancellationPolicies.Commands.AddPolicyRule;
using FlightManagement.Application.Features.CancellationPolicies.Commands.CreateCancellationPolicy;
using FlightManagement.Application.Features.CancellationPolicies.Commands.DeleteCancellationPolicy;
using FlightManagement.Application.Features.CancellationPolicies.Commands.RemovePolicyRule;
using FlightManagement.Application.Features.CancellationPolicies.Commands.UpdateCancellationPolicy;
using FlightManagement.Application.Features.CancellationPolicies.Queries.GetCancellationPolicies;
using FlightManagement.Application.Features.CancellationPolicies.Queries.GetCancellationPolicyById;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Admin;

[RequireAdmin]
[Route("api/admin/cancellation-policies")]
public class CancellationPoliciesController : BaseApiController
{
    /// <summary>
    /// Get all cancellation policies.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] bool? isActive, [FromQuery] bool? isRefundable)
    {
        var result = await Mediator.Send(new GetCancellationPoliciesQuery(isActive, isRefundable));
        return HandleResult(result);
    }

    /// <summary>
    /// Get cancellation policy by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetCancellationPolicyByIdQuery(id));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Create a new cancellation policy.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCancellationPolicyCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Data });
    }

    /// <summary>
    /// Update an existing cancellation policy.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCancellationPolicyCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Delete a cancellation policy.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await Mediator.Send(new DeleteCancellationPolicyCommand(id));
        return HandleResult(result);
    }

    /// <summary>
    /// Add a rule to a cancellation policy.
    /// </summary>
    [HttpPost("{policyId:guid}/rules")]
    public async Task<IActionResult> AddRule(Guid policyId, [FromBody] AddPolicyRuleRequest request)
    {
        var command = new AddPolicyRuleCommand(
            policyId,
            request.MinHoursBeforeDeparture,
            request.MaxHoursBeforeDeparture,
            request.RefundPercentage,
            request.FlatFee,
            request.Currency ?? "USD"
        );

        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = policyId });
    }

    /// <summary>
    /// Remove a rule from a cancellation policy.
    /// </summary>
    [HttpDelete("{policyId:guid}/rules/{ruleId:guid}")]
    public async Task<IActionResult> RemoveRule(Guid policyId, Guid ruleId)
    {
        var result = await Mediator.Send(new RemovePolicyRuleCommand(ruleId));
        return HandleResult(result);
    }
}

public record AddPolicyRuleRequest(
    int MinHoursBeforeDeparture,
    int? MaxHoursBeforeDeparture,
    decimal RefundPercentage,
    decimal FlatFee,
    string? Currency
);

