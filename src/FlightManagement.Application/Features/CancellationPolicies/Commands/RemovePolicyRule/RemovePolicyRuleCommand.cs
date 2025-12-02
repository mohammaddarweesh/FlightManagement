using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.RemovePolicyRule;

/// <summary>
/// Command to remove a rule from a cancellation policy.
/// </summary>
public record RemovePolicyRuleCommand(Guid RuleId) : ICommand;

