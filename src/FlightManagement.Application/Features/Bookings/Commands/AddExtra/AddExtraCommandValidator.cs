using FluentValidation;

namespace FlightManagement.Application.Features.Bookings.Commands.AddExtra;

/// <summary>
/// Validator for AddExtraCommand.
/// </summary>
public class AddExtraCommandValidator : AbstractValidator<AddExtraCommand>
{
    public AddExtraCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID is required");

        RuleFor(x => x.ExtraType)
            .IsInEnum().WithMessage("Invalid extra type");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than 0")
            .LessThanOrEqualTo(10).WithMessage("Quantity cannot exceed 10");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Unit price cannot be negative");
    }
}

