using FluentValidation;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.UpdateCancellationPolicy;

public class UpdateCancellationPolicyCommandValidator : AbstractValidator<UpdateCancellationPolicyCommand>
{
    public UpdateCancellationPolicyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Policy ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
    }
}

