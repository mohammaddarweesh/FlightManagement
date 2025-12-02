using FluentValidation;

namespace FlightManagement.Application.Features.Users.Commands.RequestPasswordReset;

/// <summary>
/// Validator for RequestPasswordResetCommand.
/// Ensures password reset request data meets required constraints.
/// </summary>
public class RequestPasswordResetCommandValidator : AbstractValidator<RequestPasswordResetCommand>
{
    public RequestPasswordResetCommandValidator()
    {
        // Email: required, valid format
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(256).WithMessage("Email cannot exceed 256 characters");
    }
}

