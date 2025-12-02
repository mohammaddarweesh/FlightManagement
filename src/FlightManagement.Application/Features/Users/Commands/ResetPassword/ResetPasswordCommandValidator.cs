using FluentValidation;

namespace FlightManagement.Application.Features.Users.Commands.ResetPassword;

/// <summary>
/// Validator for ResetPasswordCommand.
/// Ensures password reset data meets required constraints.
/// </summary>
public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        // Token: required
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Reset token is required");

        // NewPassword: required, minimum 8 characters, must contain uppercase, lowercase, digit, special char
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters")
            .MaximumLength(128).WithMessage("Password cannot exceed 128 characters")
            .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("Password must contain at least one digit")
            .Matches("[^a-zA-Z0-9]").WithMessage("Password must contain at least one special character");

        // ConfirmPassword: must match NewPassword
        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Confirm password is required")
            .Equal(x => x.NewPassword).WithMessage("Passwords do not match");
    }
}

