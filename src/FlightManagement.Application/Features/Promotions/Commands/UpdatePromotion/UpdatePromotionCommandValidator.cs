using FluentValidation;

namespace FlightManagement.Application.Features.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionCommandValidator : AbstractValidator<UpdatePromotionCommand>
{
    public UpdatePromotionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Promotion ID is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Promotion name is required")
            .MaximumLength(200).WithMessage("Promotion name cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");

        RuleFor(x => x.DiscountValue)
            .GreaterThan(0).WithMessage("Discount value must be greater than 0");

        RuleFor(x => x.MaxDiscountAmount)
            .GreaterThan(0).When(x => x.MaxDiscountAmount.HasValue)
            .WithMessage("Maximum discount amount must be greater than 0");

        RuleFor(x => x.MinBookingAmount)
            .GreaterThan(0).When(x => x.MinBookingAmount.HasValue)
            .WithMessage("Minimum booking amount must be greater than 0");

        RuleFor(x => x.ValidFrom)
            .NotEmpty().WithMessage("Valid from date is required");

        RuleFor(x => x.ValidTo)
            .NotEmpty().WithMessage("Valid to date is required")
            .GreaterThan(x => x.ValidFrom).WithMessage("Valid to date must be after valid from date");

        RuleFor(x => x.MaxTotalUses)
            .GreaterThan(0).When(x => x.MaxTotalUses.HasValue)
            .WithMessage("Maximum total uses must be greater than 0");

        RuleFor(x => x.MaxUsesPerCustomer)
            .GreaterThan(0).When(x => x.MaxUsesPerCustomer.HasValue)
            .WithMessage("Maximum uses per customer must be greater than 0");
    }
}

