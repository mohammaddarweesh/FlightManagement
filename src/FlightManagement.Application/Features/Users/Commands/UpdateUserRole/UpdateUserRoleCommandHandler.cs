using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Users.Commands.UpdateUserRole;

public class UpdateUserRoleCommandHandler : ICommandHandler<UpdateUserRoleCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserRoleCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateUserRoleCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<User>();

        var user = await repository.GetByIdAsync(request.UserId, cancellationToken);
        if (user == null)
        {
            return Result.Failure($"User with ID '{request.UserId}' not found.");
        }

        user.UserType = request.NewUserType;

        repository.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

