using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.CancellationPolicies.Queries.GetCancellationPolicies;

namespace FlightManagement.Application.Features.CancellationPolicies.Queries.GetCancellationPolicyById;

/// <summary>
/// Query to get a cancellation policy by ID.
/// </summary>
public record GetCancellationPolicyByIdQuery(Guid Id) : IQuery<CancellationPolicyDto>;

