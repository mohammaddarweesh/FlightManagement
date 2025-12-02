using FluentValidation;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.RemovePolicyRule;

public class RemovePolicyRuleCommandValidator : AbstractValidator<RemovePolicyRuleCommand>
{
    public RemovePolicyRuleCommandValidator()
    {
        RuleFor(x => x.RuleId)
            .NotEmpty().WithMessage("Rule ID is required");
    }
}

