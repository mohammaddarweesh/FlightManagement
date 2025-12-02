using FluentValidation;

namespace FlightManagement.Application.Features.Seats.Commands.ReserveSeat;

/// <summary>
/// Validator for ReserveSeatCommand.
/// Ensures seat reservation data meets required constraints.
/// </summary>
public class ReserveSeatCommandValidator : AbstractValidator<ReserveSeatCommand>
{
    public ReserveSeatCommandValidator()
    {
        // FlightId: required
        RuleFor(x => x.FlightId)
            .NotEmpty().WithMessage("Flight ID is required");

        // SeatId: required
        RuleFor(x => x.SeatId)
            .NotEmpty().WithMessage("Seat ID is required");

        // UserId: required
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User ID is required");

        // LockDurationMinutes: must be between 5 and 60 minutes
        RuleFor(x => x.LockDurationMinutes)
            .InclusiveBetween(5, 60).WithMessage("Lock duration must be between 5 and 60 minutes");
    }
}

