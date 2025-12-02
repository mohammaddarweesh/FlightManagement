using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.DeleteCancellationPolicy;

public class DeleteCancellationPolicyCommandHandler : ICommandHandler<DeleteCancellationPolicyCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCancellationPolicyCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCancellationPolicyCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<CancellationPolicy>();

        var policy = await repository.Query()
            .Include(p => p.Bookings)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (policy == null)
        {
            return Result.Failure($"Cancellation policy with ID '{request.Id}' not found.");
        }

        if (policy.Bookings.Any())
        {
            return Result.Failure("Cannot delete a cancellation policy that is associated with existing bookings.");
        }

        repository.Delete(policy);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

