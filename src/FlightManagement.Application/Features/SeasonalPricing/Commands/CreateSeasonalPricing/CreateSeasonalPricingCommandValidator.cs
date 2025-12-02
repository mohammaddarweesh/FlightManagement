using FluentValidation;

namespace FlightManagement.Application.Features.SeasonalPricing.Commands.CreateSeasonalPricing;

public class CreateSeasonalPricingCommandValidator : AbstractValidator<CreateSeasonalPricingCommand>
{
    public CreateSeasonalPricingCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.SeasonType)
            .IsInEnum().WithMessage("Invalid season type");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date");

        RuleFor(x => x.AdjustmentPercentage)
            .InclusiveBetween(-100, 500).WithMessage("Adjustment percentage must be between -100% and 500%");

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0).WithMessage("Priority must be 0 or greater");
    }
}

