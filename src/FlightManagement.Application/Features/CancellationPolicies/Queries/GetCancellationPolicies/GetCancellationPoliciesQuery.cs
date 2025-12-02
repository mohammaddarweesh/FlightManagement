using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.CancellationPolicies.Queries.GetCancellationPolicies;

/// <summary>
/// Query to get all cancellation policies.
/// </summary>
public record GetCancellationPoliciesQuery(
    bool? IsActive = null,
    bool? IsRefundable = null
) : IQuery<IEnumerable<CancellationPolicyDto>>;

public record CancellationPolicyDto(
    Guid Id,
    string Code,
    string Name,
    string Description,
    bool IsRefundable,
    bool IsActive,
    IEnumerable<CancellationPolicyRuleDto> Rules
);

public record CancellationPolicyRuleDto(
    Guid Id,
    int MinHoursBeforeDeparture,
    int? MaxHoursBeforeDeparture,
    decimal RefundPercentage,
    decimal FlatFee,
    string Currency
);

