using FluentValidation;

namespace FlightManagement.Application.Features.Policies.Commands.CreateOverbookingPolicy;

public class CreateOverbookingPolicyCommandValidator : AbstractValidator<CreateOverbookingPolicyCommand>
{
    public CreateOverbookingPolicyCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Policy name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.AirlineId)
            .NotEmpty().WithMessage("Airline ID is required");

        RuleFor(x => x.MaxOverbookingPercentage)
            .GreaterThanOrEqualTo(0).WithMessage("Max overbooking percentage must be 0 or greater")
            .LessThanOrEqualTo(50).WithMessage("Max overbooking percentage cannot exceed 50%");

        RuleFor(x => x.MaxOverbookedSeats)
            .GreaterThan(0).When(x => x.MaxOverbookedSeats.HasValue)
            .WithMessage("Max overbooked seats must be greater than 0");

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0).WithMessage("Priority must be 0 or greater");
    }
}

