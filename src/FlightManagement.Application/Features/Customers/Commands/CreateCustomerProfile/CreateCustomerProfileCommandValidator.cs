using FluentValidation;

namespace FlightManagement.Application.Features.Customers.Commands.CreateCustomerProfile;

/// <summary>
/// Validator for CreateCustomerProfileCommand.
/// Ensures customer profile data meets required constraints.
/// </summary>
public class CreateCustomerProfileCommandValidator : AbstractValidator<CreateCustomerProfileCommand>
{
    public CreateCustomerProfileCommandValidator()
    {
        // UserId: required
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        // FirstName: required, max 100 characters
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters")
            .Matches("^[a-zA-Z\\s'-]+$").WithMessage("First name can only contain letters, spaces, hyphens, and apostrophes");

        // LastName: required, max 100 characters
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters")
            .Matches("^[a-zA-Z\\s'-]+$").WithMessage("Last name can only contain letters, spaces, hyphens, and apostrophes");

        // PhoneNumber: optional, but if provided must be valid format
        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Invalid phone number format (use E.164 format, e.g., +12025551234)")
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

        // DateOfBirth: optional, but if provided must be in the past and user must be at least 18
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.UtcNow.Date).WithMessage("Date of birth must be in the past")
            .Must(BeAtLeast18YearsOld).WithMessage("User must be at least 18 years old")
            .When(x => x.DateOfBirth.HasValue);

        // AddressLine1: optional, max 200 characters
        RuleFor(x => x.AddressLine1)
            .MaximumLength(200).WithMessage("Address line 1 cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.AddressLine1));

        // AddressLine2: optional, max 200 characters
        RuleFor(x => x.AddressLine2)
            .MaximumLength(200).WithMessage("Address line 2 cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.AddressLine2));

        // City: optional, max 100 characters
        RuleFor(x => x.City)
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.City));

        // State: optional, max 100 characters
        RuleFor(x => x.State)
            .MaximumLength(100).WithMessage("State cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.State));

        // PostalCode: optional, max 20 characters
        RuleFor(x => x.PostalCode)
            .MaximumLength(20).WithMessage("Postal code cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.PostalCode));

        // Country: optional, max 100 characters
        RuleFor(x => x.Country)
            .MaximumLength(100).WithMessage("Country cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.Country));

        // PreferredLanguage: optional, ISO 639-1 format (2 characters)
        RuleFor(x => x.PreferredLanguage)
            .Length(2).WithMessage("Preferred language must be a 2-character ISO 639-1 code")
            .Matches("^[a-z]{2}$").WithMessage("Preferred language must be lowercase letters only")
            .When(x => !string.IsNullOrEmpty(x.PreferredLanguage));

        // PreferredCurrency: optional, ISO 4217 format (3 characters)
        RuleFor(x => x.PreferredCurrency)
            .Length(3).WithMessage("Preferred currency must be a 3-character ISO 4217 code")
            .Matches("^[A-Z]{3}$").WithMessage("Preferred currency must be uppercase letters only")
            .When(x => !string.IsNullOrEmpty(x.PreferredCurrency));
    }

    private static bool BeAtLeast18YearsOld(DateTime? dateOfBirth)
    {
        if (!dateOfBirth.HasValue) return true;
        var today = DateTime.UtcNow.Date;
        var age = today.Year - dateOfBirth.Value.Year;
        if (dateOfBirth.Value.Date > today.AddYears(-age)) age--;
        return age >= 18;
    }
}

