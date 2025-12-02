using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.CancellationPolicies.Queries.GetCancellationPolicies;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.CancellationPolicies.Queries.GetCancellationPolicyById;

public class GetCancellationPolicyByIdQueryHandler : IQueryHandler<GetCancellationPolicyByIdQuery, CancellationPolicyDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetCancellationPolicyByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CancellationPolicyDto>> Handle(
        GetCancellationPolicyByIdQuery request,
        CancellationToken cancellationToken)
    {
        var policy = await _unitOfWork.Repository<CancellationPolicy>()
            .Query()
            .Include(p => p.Rules)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (policy == null)
        {
            return Result<CancellationPolicyDto>.Failure($"Cancellation policy with ID '{request.Id}' not found.");
        }

        var dto = new CancellationPolicyDto(
            policy.Id,
            policy.Code,
            policy.Name,
            policy.Description,
            policy.IsRefundable,
            policy.IsActive,
            policy.Rules.OrderBy(r => r.MinHoursBeforeDeparture).Select(r => new CancellationPolicyRuleDto(
                r.Id,
                r.MinHoursBeforeDeparture,
                r.MaxHoursBeforeDeparture,
                r.RefundPercentage,
                r.FlatFee,
                r.Currency
            ))
        );

        return Result<CancellationPolicyDto>.Success(dto);
    }
}

