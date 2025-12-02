using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.UpdateCancellationPolicy;

public class UpdateCancellationPolicyCommandHandler : ICommandHandler<UpdateCancellationPolicyCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateCancellationPolicyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateCancellationPolicyCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<CancellationPolicy>();

        var policy = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (policy == null)
        {
            return Result.Failure($"Cancellation policy with ID '{request.Id}' not found.");
        }

        policy.Name = request.Name;
        policy.Description = request.Description;
        policy.IsRefundable = request.IsRefundable;
        policy.IsActive = request.IsActive;

        repository.Update(policy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

