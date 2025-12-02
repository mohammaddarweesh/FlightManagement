using FluentValidation;

namespace FlightManagement.Application.Features.Airports.Commands.CreateAirport;

/// <summary>
/// Validator for CreateAirportCommand.
/// Ensures airport data meets required constraints.
/// </summary>
public class CreateAirportCommandValidator : AbstractValidator<CreateAirportCommand>
{
    public CreateAirportCommandValidator()
    {
        // IATA code: exactly 3 uppercase letters
        RuleFor(x => x.IataCode)
            .NotEmpty().WithMessage("IATA code is required")
            .Length(3).WithMessage("IATA code must be exactly 3 characters")
            .Matches("^[A-Z]{3}$").WithMessage("IATA code must contain only uppercase letters");

        // ICAO code: exactly 4 uppercase letters
        RuleFor(x => x.IcaoCode)
            .NotEmpty().WithMessage("ICAO code is required")
            .Length(4).WithMessage("ICAO code must be exactly 4 characters")
            .Matches("^[A-Z]{4}$").WithMessage("ICAO code must contain only uppercase letters");

        // Name: required, max 200 characters
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Airport name is required")
            .MaximumLength(200).WithMessage("Airport name cannot exceed 200 characters");

        // City: required, max 100 characters
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters");

        // Country: required, max 100 characters
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100).WithMessage("Country cannot exceed 100 characters");

        // CountryCode: exactly 2 uppercase letters (ISO 3166-1 alpha-2)
        RuleFor(x => x.CountryCode)
            .NotEmpty().WithMessage("Country code is required")
            .Length(2).WithMessage("Country code must be exactly 2 characters")
            .Matches("^[A-Z]{2}$").WithMessage("Country code must be a valid ISO 3166-1 alpha-2 code");

        // Timezone: required, valid IANA timezone format
        RuleFor(x => x.Timezone)
            .NotEmpty().WithMessage("Timezone is required")
            .MaximumLength(50).WithMessage("Timezone cannot exceed 50 characters")
            .Matches("^[A-Za-z_/]+$").WithMessage("Timezone must be a valid IANA timezone identifier");

        // Latitude: -90 to 90
        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90");

        // Longitude: -180 to 180
        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180");
    }
}

