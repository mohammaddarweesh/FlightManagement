using FluentValidation;

namespace FlightManagement.Application.Features.Auth.Commands.Login;

/// <summary>
/// Validator for LoginCommand.
/// Ensures login data meets required constraints.
/// </summary>
public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        // Email: required, valid format
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format");

        // Password: required
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}

