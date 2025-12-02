using FluentValidation;

namespace FlightManagement.Application.Features.Users.Commands.UpdateUserRole;

public class UpdateUserRoleCommandValidator : AbstractValidator<UpdateUserRoleCommand>
{
    public UpdateUserRoleCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        RuleFor(x => x.NewUserType)
            .IsInEnum().WithMessage("Invalid user type");
    }
}

