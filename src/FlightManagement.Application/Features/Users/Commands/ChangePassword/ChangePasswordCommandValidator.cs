using FluentValidation;

namespace FlightManagement.Application.Features.Users.Commands.ChangePassword;

/// <summary>
/// Validator for ChangePasswordCommand.
/// Ensures password change data meets required constraints.
/// </summary>
public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        // UserId: required
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        // CurrentPassword: required
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required");

        // NewPassword: required, minimum 8 characters, must contain uppercase, lowercase, digit, special char
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .MaximumLength(128).WithMessage("Password cannot exceed 128 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character")
            .NotEqual(x => x.CurrentPassword).WithMessage("New password must be different from current password");

        // ConfirmPassword: must match NewPassword
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required")
            .Equal(x => x.NewPassword).WithMessage("Passwords do not match");
    }
}

