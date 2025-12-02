using FluentValidation;

namespace FlightManagement.Application.Features.SeasonalPricing.Commands.DeleteSeasonalPricing;

public class DeleteSeasonalPricingCommandValidator : AbstractValidator<DeleteSeasonalPricingCommand>
{
    public DeleteSeasonalPricingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ID is required");
    }
}

