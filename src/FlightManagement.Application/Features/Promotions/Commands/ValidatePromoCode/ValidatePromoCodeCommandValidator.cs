using FluentValidation;

namespace FlightManagement.Application.Features.Promotions.Commands.ValidatePromoCode;

public class ValidatePromoCodeCommandValidator : AbstractValidator<ValidatePromoCodeCommand>
{
    public ValidatePromoCodeCommandValidator()
    {
        RuleFor(x => x.PromoCode)
            .NotEmpty().WithMessage("Promo code is required")
            .MaximumLength(50).WithMessage("Promo code cannot exceed 50 characters");

        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage("Customer ID is required");

        RuleFor(x => x.BookingAmount)
            .GreaterThan(0).WithMessage("Booking amount must be greater than 0");

        RuleFor(x => x.Flights)
            .NotEmpty().WithMessage("At least one flight is required");
    }
}

