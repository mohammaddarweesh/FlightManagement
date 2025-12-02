using FluentValidation;

namespace FlightManagement.Application.Features.Users.Commands.VerifyEmail;

/// <summary>
/// Validator for VerifyEmailCommand.
/// Ensures email verification data meets required constraints.
/// </summary>
public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
{
    public VerifyEmailCommandValidator()
    {
        // Token: required
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Verification token is required");
    }
}

