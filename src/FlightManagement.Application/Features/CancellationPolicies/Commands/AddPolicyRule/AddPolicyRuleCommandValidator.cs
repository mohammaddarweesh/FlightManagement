using FluentValidation;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.AddPolicyRule;

public class AddPolicyRuleCommandValidator : AbstractValidator<AddPolicyRuleCommand>
{
    public AddPolicyRuleCommandValidator()
    {
        RuleFor(x => x.CancellationPolicyId)
            .NotEmpty().WithMessage("Cancellation policy ID is required");

        RuleFor(x => x.MinHoursBeforeDeparture)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum hours before departure must be 0 or greater");

        RuleFor(x => x.MaxHoursBeforeDeparture)
            .GreaterThan(x => x.MinHoursBeforeDeparture)
            .When(x => x.MaxHoursBeforeDeparture.HasValue)
            .WithMessage("Maximum hours must be greater than minimum hours");

        RuleFor(x => x.RefundPercentage)
            .InclusiveBetween(0, 100).WithMessage("Refund percentage must be between 0 and 100");

        RuleFor(x => x.FlatFee)
            .GreaterThanOrEqualTo(0).WithMessage("Flat fee must be 0 or greater");

        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Currency is required")
            .Length(3).WithMessage("Currency must be a 3-letter code");
    }
}

