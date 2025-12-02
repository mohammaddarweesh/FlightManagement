using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.CreateCancellationPolicy;

/// <summary>
/// Command to create a new cancellation policy.
/// </summary>
public record CreateCancellationPolicyCommand(
    string Code,
    string Name,
    string Description,
    bool IsRefundable,
    bool IsActive = true
) : ICommand<Guid>;

