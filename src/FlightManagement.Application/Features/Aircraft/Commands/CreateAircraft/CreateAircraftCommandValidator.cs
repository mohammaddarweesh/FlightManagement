using FluentValidation;

namespace FlightManagement.Application.Features.Aircraft.Commands.CreateAircraft;

/// <summary>
/// Validator for CreateAircraftCommand.
/// Ensures aircraft data meets required constraints.
/// </summary>
public class CreateAircraftCommandValidator : AbstractValidator<CreateAircraftCommand>
{
    public CreateAircraftCommandValidator()
    {
        // AirlineId: required
        RuleFor(x => x.AirlineId)
            .NotEmpty().WithMessage("Airline ID is required");

        // Model: required, max 100 characters
        RuleFor(x => x.Model)
            .NotEmpty().WithMessage("Aircraft model is required")
            .MaximumLength(100).WithMessage("Aircraft model cannot exceed 100 characters");

        // Manufacturer: required, max 100 characters
        RuleFor(x => x.Manufacturer)
            .NotEmpty().WithMessage("Manufacturer is required")
            .MaximumLength(100).WithMessage("Manufacturer cannot exceed 100 characters");

        // RegistrationNumber: required, max 20 characters, alphanumeric with dashes
        RuleFor(x => x.RegistrationNumber)
            .NotEmpty().WithMessage("Registration number is required")
            .MaximumLength(20).WithMessage("Registration number cannot exceed 20 characters")
            .Matches("^[A-Z0-9-]+$").WithMessage("Registration number must contain only uppercase letters, numbers, or dashes");

        // CabinClasses: at least one cabin class required
        RuleFor(x => x.CabinClasses)
            .NotEmpty().WithMessage("At least one cabin class configuration is required")
            .Must(classes => classes.Count > 0).WithMessage("At least one cabin class configuration is required");

        // Validate each cabin class
        RuleForEach(x => x.CabinClasses).ChildRules(cabin =>
        {
            cabin.RuleFor(c => c.RowStart)
                .GreaterThan(0).WithMessage("Row start must be greater than 0");

            cabin.RuleFor(c => c.RowEnd)
                .GreaterThan(0).WithMessage("Row end must be greater than 0")
                .GreaterThanOrEqualTo(c => c.RowStart).WithMessage("Row end must be greater than or equal to row start");

            cabin.RuleFor(c => c.SeatLayout)
                .NotEmpty().WithMessage("Seat layout is required")
                .Matches("^[A-Z]+-[A-Z]+$|^[A-Z]+$").WithMessage("Seat layout must be letters with optional dash (e.g., 'ABC-DEF' or 'ABCDEF')");

            cabin.RuleFor(c => c.BasePriceMultiplier)
                .GreaterThan(0).WithMessage("Base price multiplier must be greater than 0");
        });
    }
}

