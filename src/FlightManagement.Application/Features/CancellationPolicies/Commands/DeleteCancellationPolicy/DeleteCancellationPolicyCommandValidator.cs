using FluentValidation;

namespace FlightManagement.Application.Features.CancellationPolicies.Commands.DeleteCancellationPolicy;

public class DeleteCancellationPolicyCommandValidator : AbstractValidator<DeleteCancellationPolicyCommand>
{
    public DeleteCancellationPolicyCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Policy ID is required");
    }
}

