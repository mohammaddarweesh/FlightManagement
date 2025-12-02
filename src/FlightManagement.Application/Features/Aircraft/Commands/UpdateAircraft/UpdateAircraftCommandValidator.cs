using FluentValidation;

namespace FlightManagement.Application.Features.Aircraft.Commands.UpdateAircraft;

/// <summary>
/// Validator for UpdateAircraftCommand.
/// Ensures aircraft update data meets required constraints.
/// </summary>
public class UpdateAircraftCommandValidator : AbstractValidator<UpdateAircraftCommand>
{
    public UpdateAircraftCommandValidator()
    {
        // Id: required
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Aircraft ID is required");

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
    }
}

