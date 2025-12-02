using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.CancellationPolicies.Queries.GetCancellationPolicies;

public class GetCancellationPoliciesQueryHandler : IQueryHandler<GetCancellationPoliciesQuery, IEnumerable<CancellationPolicyDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCancellationPoliciesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<CancellationPolicyDto>>> Handle(
        GetCancellationPoliciesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<CancellationPolicy>().Query();

        if (request.IsActive.HasValue)
            query = query.Where(p => p.IsActive == request.IsActive.Value);

        if (request.IsRefundable.HasValue)
            query = query.Where(p => p.IsRefundable == request.IsRefundable.Value);

        var policies = await query
            .Include(p => p.Rules)
            .OrderBy(p => p.Code)
            .ToListAsync(cancellationToken);

        var dtos = policies.Select(p => new CancellationPolicyDto(
            p.Id,
            p.Code,
            p.Name,
            p.Description,
            p.IsRefundable,
            p.IsActive,
            p.Rules.OrderBy(r => r.MinHoursBeforeDeparture).Select(r => new CancellationPolicyRuleDto(
                r.Id,
                r.MinHoursBeforeDeparture,
                r.MaxHoursBeforeDeparture,
                r.RefundPercentage,
                r.FlatFee,
                r.Currency
            ))
        ));

        return Result<IEnumerable<CancellationPolicyDto>>.Success(dtos);
    }
}

