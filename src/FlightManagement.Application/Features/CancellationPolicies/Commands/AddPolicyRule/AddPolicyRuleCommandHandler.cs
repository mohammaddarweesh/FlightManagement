using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.AddPolicyRule;

public class AddPolicyRuleCommandHandler : ICommandHandler<AddPolicyRuleCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddPolicyRuleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(AddPolicyRuleCommand request, CancellationToken cancellationToken)
    {
        var policyRepository = _unitOfWork.Repository<CancellationPolicy>();
        var ruleRepository = _unitOfWork.Repository<CancellationPolicyRule>();

        var policy = await policyRepository.GetByIdAsync(request.CancellationPolicyId, cancellationToken);
        if (policy == null)
        {
            return Result<Guid>.Failure($"Cancellation policy with ID '{request.CancellationPolicyId}' not found.");
        }

        var rule = new CancellationPolicyRule
        {
            CancellationPolicyId = request.CancellationPolicyId,
            MinHoursBeforeDeparture = request.MinHoursBeforeDeparture,
            MaxHoursBeforeDeparture = request.MaxHoursBeforeDeparture,
            RefundPercentage = request.RefundPercentage,
            FlatFee = request.FlatFee,
            Currency = request.Currency
        };

        await ruleRepository.AddAsync(rule, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(rule.Id);
    }
}

