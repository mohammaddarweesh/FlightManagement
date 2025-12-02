using FluentValidation;

namespace FlightManagement.Application.Features.Bookings.Commands.UpdatePassenger;

/// <summary>
/// Validator for UpdatePassengerCommand.
/// </summary>
public class UpdatePassengerCommandValidator : AbstractValidator<UpdatePassengerCommand>
{
    public UpdatePassengerCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID is required");

        RuleFor(x => x.PassengerId)
            .NotEmpty().WithMessage("Passenger ID is required");

        RuleFor(x => x.Title)
            .MaximumLength(10).WithMessage("Title cannot exceed 10 characters")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.FirstName)
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.FirstName));

        RuleFor(x => x.MiddleName)
            .MaximumLength(100).WithMessage("Middle name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.MiddleName));

        RuleFor(x => x.LastName)
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.LastName));

        RuleFor(x => x.PassportNumber)
            .MaximumLength(20).WithMessage("Passport number cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.PassportNumber));

        RuleFor(x => x.PassportIssuingCountry)
            .Length(2).WithMessage("Passport issuing country must be a 2-character ISO code")
            .When(x => !string.IsNullOrEmpty(x.PassportIssuingCountry));

        RuleFor(x => x.PassportExpiryDate)
            .GreaterThan(DateTime.UtcNow.Date).WithMessage("Passport must not be expired")
            .When(x => x.PassportExpiryDate.HasValue);

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Invalid email format")
            .MaximumLength(255).WithMessage("Email cannot exceed 255 characters")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Phone)
            .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.Phone));

        RuleFor(x => x.MealPreference)
            .IsInEnum().WithMessage("Invalid meal preference")
            .When(x => x.MealPreference.HasValue);

        RuleFor(x => x.SpecialAssistance)
            .MaximumLength(500).WithMessage("Special assistance cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.SpecialAssistance));

        RuleFor(x => x.FrequentFlyerNumber)
            .MaximumLength(20).WithMessage("Frequent flyer number cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.FrequentFlyerNumber));
    }
}

