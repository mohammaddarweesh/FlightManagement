using FluentValidation;

namespace FlightManagement.Application.Features.Policies.Commands.CreateBlackoutDate;

public class CreateBlackoutDateCommandValidator : AbstractValidator<CreateBlackoutDateCommand>
{
    public CreateBlackoutDateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Blackout date name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThanOrEqualTo(x => x.StartDate).WithMessage("End date must be on or after start date");

        RuleFor(x => x)
            .Must(x => x.BlocksBookings || x.BlocksPromotions)
            .WithMessage("Blackout date must block either bookings or promotions (or both)");
    }
}

