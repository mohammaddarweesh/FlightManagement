using FluentValidation;

namespace FlightManagement.Application.Features.Policies.Commands.CreateBookingPolicy;

public class CreateBookingPolicyCommandValidator : AbstractValidator<CreateBookingPolicyCommand>
{
    public CreateBookingPolicyCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Policy code is required")
            .MaximumLength(50).WithMessage("Policy code cannot exceed 50 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Policy name is required")
            .MaximumLength(200).WithMessage("Policy name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.Value)
            .GreaterThan(0).WithMessage("Policy value must be greater than 0");

        RuleFor(x => x.ErrorMessage)
            .NotEmpty().WithMessage("Error message is required")
            .MaximumLength(500).WithMessage("Error message cannot exceed 500 characters");

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0).WithMessage("Priority must be 0 or greater");
    }
}

