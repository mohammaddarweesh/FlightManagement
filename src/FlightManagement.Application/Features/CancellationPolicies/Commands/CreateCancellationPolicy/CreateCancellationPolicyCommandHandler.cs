using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.CreateCancellationPolicy;

public class CreateCancellationPolicyCommandHandler : ICommandHandler<CreateCancellationPolicyCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateCancellationPolicyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateCancellationPolicyCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<CancellationPolicy>();

        // Check for duplicate code
        var existingPolicy = await repository.FirstOrDefaultAsync(
            p => p.Code == request.Code,
            cancellationToken);

        if (existingPolicy != null)
        {
            return Result<Guid>.Failure($"A cancellation policy with code '{request.Code}' already exists.");
        }

        var policy = new CancellationPolicy
        {
            Code = request.Code,
            Name = request.Name,
            Description = request.Description,
            IsRefundable = request.IsRefundable,
            IsActive = request.IsActive
        };

        await repository.AddAsync(policy, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(policy.Id);
    }
}

