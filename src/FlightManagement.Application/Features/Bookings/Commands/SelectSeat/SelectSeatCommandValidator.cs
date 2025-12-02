using FluentValidation;

namespace FlightManagement.Application.Features.Bookings.Commands.SelectSeat;

/// <summary>
/// Validator for SelectSeatCommand.
/// </summary>
public class SelectSeatCommandValidator : AbstractValidator<SelectSeatCommand>
{
    public SelectSeatCommandValidator()
    {
        RuleFor(x => x.BookingId)
            .NotEmpty().WithMessage("Booking ID is required");

        RuleFor(x => x.PassengerId)
            .NotEmpty().WithMessage("Passenger ID is required");

        RuleFor(x => x.BookingSegmentId)
            .NotEmpty().WithMessage("Booking segment ID is required");

        RuleFor(x => x.FlightSeatId)
            .NotEmpty().WithMessage("Flight seat ID is required");
    }
}

