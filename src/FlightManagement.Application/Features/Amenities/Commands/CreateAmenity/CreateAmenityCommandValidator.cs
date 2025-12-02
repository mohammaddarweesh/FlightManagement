using FluentValidation;

namespace FlightManagement.Application.Features.Amenities.Commands.CreateAmenity;

/// <summary>
/// Validator for CreateAmenityCommand.
/// Ensures amenity data meets required constraints.
/// </summary>
public class CreateAmenityCommandValidator : AbstractValidator<CreateAmenityCommand>
{
    public CreateAmenityCommandValidator()
    {
        // Code: required, uppercase, max 20 characters
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Amenity code is required")
            .MaximumLength(20).WithMessage("Amenity code cannot exceed 20 characters")
            .Matches("^[A-Z0-9_]+$").WithMessage("Amenity code must contain only uppercase letters, numbers, or underscores");

        // Name: required, max 100 characters
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Amenity name is required")
            .MaximumLength(100).WithMessage("Amenity name cannot exceed 100 characters");

        // Description: required, max 500 characters
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

        // Category: must be a valid enum value
        RuleFor(x => x.Category)
            .IsInEnum().WithMessage("Invalid amenity category");

        // IconUrl: optional, but if provided must be valid URL
        RuleFor(x => x.IconUrl)
            .Must(BeValidUrlOrNull).WithMessage("Icon URL must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.IconUrl));
    }

    private static bool BeValidUrlOrNull(string? url)
    {
        if (string.IsNullOrEmpty(url)) return true;
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}

