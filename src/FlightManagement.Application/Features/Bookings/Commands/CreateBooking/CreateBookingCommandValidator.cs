using FluentValidation;

namespace FlightManagement.Application.Features.Bookings.Commands.CreateBooking;

/// <summary>
/// Validator for CreateBookingCommand.
/// </summary>
public class CreateBookingCommandValidator : AbstractValidator<CreateBookingCommand>
{
    public CreateBookingCommandValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required");

        RuleFor(x => x.TripType)
            .IsInEnum().WithMessage("Invalid trip type");

        RuleFor(x => x.ContactEmail)
            .NotEmpty().WithMessage("Contact email is required")
            .MaximumLength(255).WithMessage("Contact email cannot exceed 255 characters")
            .EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.ContactPhone)
            .NotEmpty().WithMessage("Contact phone is required")
            .MaximumLength(20).WithMessage("Contact phone cannot exceed 20 characters");

        RuleFor(x => x.SpecialRequests)
            .MaximumLength(1000).WithMessage("Special requests cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.SpecialRequests));

        RuleFor(x => x.PromoCode)
            .MaximumLength(50).WithMessage("Promo code cannot exceed 50 characters")
            .When(x => !string.IsNullOrEmpty(x.PromoCode));

        // Segments validation
        RuleFor(x => x.Segments)
            .NotEmpty().WithMessage("At least one flight segment is required")
            .Must(s => s.Count <= 10).WithMessage("Cannot have more than 10 segments");

        RuleForEach(x => x.Segments).SetValidator(new BookingSegmentInputValidator());

        // Passengers validation
        RuleFor(x => x.Passengers)
            .NotEmpty().WithMessage("At least one passenger is required")
            .Must(p => p.Count <= 9).WithMessage("Cannot have more than 9 passengers")
            .Must(HaveExactlyOneLeadPassenger).WithMessage("Exactly one lead passenger is required")
            .Must(HaveAtLeastOneAdult).WithMessage("At least one adult passenger is required");

        RuleForEach(x => x.Passengers).SetValidator(new PassengerInputValidator());
    }

    private bool HaveExactlyOneLeadPassenger(List<PassengerInput> passengers)
    {
        return passengers.Count(p => p.IsLeadPassenger) == 1;
    }

    private bool HaveAtLeastOneAdult(List<PassengerInput> passengers)
    {
        return passengers.Any(p => p.PassengerType == Domain.Enums.PassengerType.Adult);
    }
}

/// <summary>
/// Validator for BookingSegmentInput.
/// </summary>
public class BookingSegmentInputValidator : AbstractValidator<BookingSegmentInput>
{
    public BookingSegmentInputValidator()
    {
        RuleFor(x => x.FlightId)
            .NotEmpty().WithMessage("Flight ID is required");

        RuleFor(x => x.CabinClass)
            .IsInEnum().WithMessage("Invalid cabin class");

        RuleFor(x => x.SegmentOrder)
            .GreaterThan(0).WithMessage("Segment order must be greater than 0");
    }
}

/// <summary>
/// Validator for PassengerInput.
/// </summary>
public class PassengerInputValidator : AbstractValidator<PassengerInput>
{
    public PassengerInputValidator()
    {
        RuleFor(x => x.PassengerType)
            .IsInEnum().WithMessage("Invalid passenger type");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(10).WithMessage("Title cannot exceed 10 characters");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(100).WithMessage("First name cannot exceed 100 characters");

        RuleFor(x => x.MiddleName)
            .MaximumLength(100).WithMessage("Middle name cannot exceed 100 characters")
            .When(x => !string.IsNullOrEmpty(x.MiddleName));

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(100).WithMessage("Last name cannot exceed 100 characters");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required")
            .LessThan(DateTime.UtcNow.Date).WithMessage("Date of birth must be in the past");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Invalid gender");

        RuleFor(x => x.Nationality)
            .NotEmpty().WithMessage("Nationality is required")
            .Length(2).WithMessage("Nationality must be a 2-character ISO country code");

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

