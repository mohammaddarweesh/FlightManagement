using FluentValidation;

namespace FlightManagement.Application.Features.Bookings.Commands.ConfirmBooking;

/// <summary>
/// Validator for ConfirmBookingCommand.
/// </summary>
public class ConfirmBookingCommandValidator : AbstractValidator<ConfirmBookingCommand>
{
    public ConfirmBookingCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID is required");

        RuleFor(x => x.TransactionReference)
            .NotEmpty().WithMessage("Transaction reference is required")
            .MaximumLength(100).WithMessage("Transaction reference cannot exceed 100 characters");

        RuleFor(x => x.PaymentMethod)
            .IsInEnum().WithMessage("Invalid payment method");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(x => x.CardLastFour)
            .Length(4).WithMessage("Card last four must be exactly 4 digits")
            .Matches("^[0-9]{4}$").WithMessage("Card last four must contain only digits")
            .When(x => !string.IsNullOrEmpty(x.CardLastFour));

        RuleFor(x => x.CardBrand)
            .MaximumLength(20).WithMessage("Card brand cannot exceed 20 characters")
            .When(x => !string.IsNullOrEmpty(x.CardBrand));
    }
}

