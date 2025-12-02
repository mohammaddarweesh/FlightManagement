using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.DeleteCancellationPolicy;

/// <summary>
/// Command to delete a cancellation policy.
/// </summary>
public record DeleteCancellationPolicyCommand(Guid Id) : ICommand;

