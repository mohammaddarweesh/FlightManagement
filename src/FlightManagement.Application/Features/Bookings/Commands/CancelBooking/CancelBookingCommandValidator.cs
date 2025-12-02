using FluentValidation;

namespace FlightManagement.Application.Features.Bookings.Commands.CancelBooking;

/// <summary>
/// Validator for CancelBookingCommand.
/// </summary>
public class CancelBookingCommandValidator : AbstractValidator<CancelBookingCommand>
{
    public CancelBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID is required");

        RuleFor(x => x.CancellationReason)
            .MaximumLength(500).WithMessage("Cancellation reason cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.CancellationReason));
    }
}

