using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.RemovePolicyRule;

public class RemovePolicyRuleCommandHandler : ICommandHandler<RemovePolicyRuleCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public RemovePolicyRuleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RemovePolicyRuleCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<CancellationPolicyRule>();

        var rule = await repository.GetByIdAsync(request.RuleId, cancellationToken);
        if (rule == null)
        {
            return Result.Failure($"Cancellation policy rule with ID '{request.RuleId}' not found.");
        }

        repository.Delete(rule);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

