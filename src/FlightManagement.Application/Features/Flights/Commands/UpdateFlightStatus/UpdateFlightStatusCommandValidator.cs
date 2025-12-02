using FlightManagement.Domain.Enums;
using FluentValidation;

namespace FlightManagement.Application.Features.Flights.Commands.UpdateFlightStatus;

/// <summary>
/// Validator for UpdateFlightStatusCommand.
/// Ensures flight status update data meets required constraints.
/// </summary>
public class UpdateFlightStatusCommandValidator : AbstractValidator<UpdateFlightStatusCommand>
{
    public UpdateFlightStatusCommandValidator()
    {
        // FlightId: required
        RuleFor(x => x.FlightId)
            .NotEmpty().WithMessage("Flight ID is required");

        // NewStatus: must be a valid enum value
        RuleFor(x => x.NewStatus)
            .IsInEnum().WithMessage("Invalid flight status");

        // ActualDepartureTime: required when status is Departed, InFlight, Landed, or Arrived
        RuleFor(x => x.ActualDepartureTime)
            .NotNull().WithMessage("Actual departure time is required when flight has departed")
            .When(x => x.NewStatus >= FlightStatus.Departed && x.NewStatus != FlightStatus.Delayed && x.NewStatus != FlightStatus.Cancelled);

        // ActualArrivalTime: required when status is Landed or Arrived
        RuleFor(x => x.ActualArrivalTime)
            .NotNull().WithMessage("Actual arrival time is required when flight has landed or arrived")
            .When(x => x.NewStatus == FlightStatus.Landed || x.NewStatus == FlightStatus.Arrived);

        // ActualArrivalTime must be after ActualDepartureTime when both are provided
        RuleFor(x => x.ActualArrivalTime)
            .GreaterThan(x => x.ActualDepartureTime).WithMessage("Actual arrival time must be after actual departure time")
            .When(x => x.ActualDepartureTime.HasValue && x.ActualArrivalTime.HasValue);
    }
}

