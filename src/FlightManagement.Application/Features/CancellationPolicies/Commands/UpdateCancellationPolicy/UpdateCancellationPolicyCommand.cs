using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.UpdateCancellationPolicy;

/// <summary>
/// Command to update an existing cancellation policy.
/// </summary>
public record UpdateCancellationPolicyCommand(
    Guid Id,
    string Name,
    string Description,
    bool IsRefundable,
    bool IsActive
) : ICommand;

