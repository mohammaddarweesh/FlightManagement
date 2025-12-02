using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Users.Commands.ActivateUser;

public class ActivateUserCommandHandler : ICommandHandler<ActivateUserCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public ActivateUserCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<User>();

        var user = await repository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Failure($"User with ID '{request.UserId}' not found.");
        }

        user.IsActive = true;

        repository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

