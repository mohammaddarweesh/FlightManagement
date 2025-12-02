using FluentValidation;

namespace FlightManagement.Application.Features.Flights.Commands.CancelFlight;

/// <summary>
/// Validator for CancelFlightCommand.
/// Ensures flight cancellation data meets required constraints.
/// </summary>
public class CancelFlightCommandValidator : AbstractValidator<CancelFlightCommand>
{
    public CancelFlightCommandValidator()
    {
        // FlightId: required
        RuleFor(x => x.FlightId)
            .NotEmpty().WithMessage("Flight ID is required");

        // CancellationReason: optional, but if provided must be max 500 characters
        RuleFor(x => x.CancellationReason)
            .MaximumLength(500).WithMessage("Cancellation reason cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.CancellationReason));
    }
}

