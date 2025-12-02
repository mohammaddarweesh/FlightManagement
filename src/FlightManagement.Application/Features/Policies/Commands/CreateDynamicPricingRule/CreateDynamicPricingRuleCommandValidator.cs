using FlightManagement.Domain.Enums;
using FluentValidation;

namespace FlightManagement.Application.Features.Policies.Commands.CreateDynamicPricingRule;

public class CreateDynamicPricingRuleCommandValidator : AbstractValidator<CreateDynamicPricingRuleCommand>
{
    public CreateDynamicPricingRuleCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Rule name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.AdjustmentPercentage)
            .InclusiveBetween(-100, 200).WithMessage("Adjustment percentage must be between -100% and 200%");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency must be a 3-letter code");

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0).WithMessage("Priority must be 0 or greater");

        // Day of week rule validation
        RuleFor(x => x.ApplicableDays)
            .NotNull().When(x => x.RuleType == PricingRuleType.DayOfWeek)
            .WithMessage("Applicable days are required for day-of-week rules");

        // Seasonal rule validation
        RuleFor(x => x.SeasonStartDate)
            .NotNull().When(x => x.RuleType == PricingRuleType.Seasonal)
            .WithMessage("Season start date is required for seasonal rules");

        RuleFor(x => x.SeasonEndDate)
            .NotNull().When(x => x.RuleType == PricingRuleType.Seasonal)
            .WithMessage("Season end date is required for seasonal rules")
            .GreaterThan(x => x.SeasonStartDate)
            .When(x => x.SeasonStartDate.HasValue && x.SeasonEndDate.HasValue)
            .WithMessage("Season end date must be after start date");

        // Demand-based rule validation
        RuleFor(x => x.MinBookingPercentage)
            .InclusiveBetween(0, 100).When(x => x.MinBookingPercentage.HasValue)
            .WithMessage("Min booking percentage must be between 0 and 100");

        RuleFor(x => x.MaxBookingPercentage)
            .InclusiveBetween(0, 100).When(x => x.MaxBookingPercentage.HasValue)
            .WithMessage("Max booking percentage must be between 0 and 100");

        // Time of day validation
        RuleFor(x => x.StartHour)
            .InclusiveBetween(0, 23).When(x => x.StartHour.HasValue)
            .WithMessage("Start hour must be between 0 and 23");

        RuleFor(x => x.EndHour)
            .InclusiveBetween(0, 23).When(x => x.EndHour.HasValue)
            .WithMessage("End hour must be between 0 and 23");
    }
}

