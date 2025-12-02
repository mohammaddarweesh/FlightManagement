using FluentValidation;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.CreateCancellationPolicy;

public class CreateCancellationPolicyCommandValidator : AbstractValidator<CreateCancellationPolicyCommand>
{
    public CreateCancellationPolicyCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required")
            .MaximumLength(50).WithMessage("Code cannot exceed 50 characters")
            .Matches("^[A-Z0-9_]+$").WithMessage("Code can only contain uppercase letters, numbers, and underscores");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
    }
}

