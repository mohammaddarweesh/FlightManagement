using FluentValidation;

namespace FlightManagement.Application.Features.Airlines.Commands.CreateAirline;

/// <summary>
/// Validator for CreateAirlineCommand.
/// Ensures airline data meets required constraints.
/// </summary>
public class CreateAirlineCommandValidator : AbstractValidator<CreateAirlineCommand>
{
    public CreateAirlineCommandValidator()
    {
        // IATA code: exactly 2 uppercase letters
        RuleFor(x => x.IataCode)
            .NotEmpty().WithMessage("IATA code is required")
            .Length(2).WithMessage("IATA code must be exactly 2 characters")
            .Matches("^[A-Z0-9]{2}$").WithMessage("IATA code must contain only uppercase letters or numbers");

        // ICAO code: exactly 3 uppercase letters
        RuleFor(x => x.IcaoCode)
            .NotEmpty().WithMessage("ICAO code is required")
            .Length(3).WithMessage("ICAO code must be exactly 3 characters")
            .Matches("^[A-Z]{3}$").WithMessage("ICAO code must contain only uppercase letters");

        // Name: required, max 200 characters
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Airline name is required")
            .MaximumLength(200).WithMessage("Airline name cannot exceed 200 characters");

        // Country: required, max 100 characters
        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100).WithMessage("Country cannot exceed 100 characters");

        // LogoUrl: optional, but if provided must be valid URL format
        RuleFor(x => x.LogoUrl)
            .Must(BeValidUrlOrNull).WithMessage("Logo URL must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.LogoUrl));
    }

    private static bool BeValidUrlOrNull(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}

