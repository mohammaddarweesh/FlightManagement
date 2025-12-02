using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.AddPolicyRule;

/// <summary>
/// Command to add a rule to a cancellation policy.
/// </summary>
public record AddPolicyRuleCommand(
    Guid CancellationPolicyId,
    int MinHoursBeforeDeparture,
    int? MaxHoursBeforeDeparture,
    decimal RefundPercentage,
    decimal FlatFee,
    string Currency = "USD"
) : ICommand<Guid>;

