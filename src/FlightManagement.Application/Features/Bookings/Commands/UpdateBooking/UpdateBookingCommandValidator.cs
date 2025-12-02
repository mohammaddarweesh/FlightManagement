using FluentValidation;

namespace FlightManagement.Application.Features.Bookings.Commands.UpdateBooking;

/// <summary>
/// Validator for UpdateBookingCommand.
/// </summary>
public class UpdateBookingCommandValidator : AbstractValidator<UpdateBookingCommand>
{
    public UpdateBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID is required");

        RuleFor(x => x.ContactEmail)
            .MaximumLength(255).WithMessage("Contact email cannot exceed 255 characters")
            .EmailAddress().WithMessage("Invalid email format")
            .When(x => !string.IsNullOrEmpty(x.ContactEmail));

        RuleFor(x => x.ContactPhone)
            .MaximumLength(20).WithMessage("Contact phone cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.ContactPhone));

        RuleFor(x => x.SpecialRequests)
            .MaximumLength(1000).WithMessage("Special requests cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.SpecialRequests));
    }
}

