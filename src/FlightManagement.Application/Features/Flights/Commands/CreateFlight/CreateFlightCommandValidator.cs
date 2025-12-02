using FluentValidation;

namespace FlightManagement.Application.Features.Flights.Commands.CreateFlight;

/// <summary>
/// Validator for CreateFlightCommand.
/// Ensures flight data meets required constraints.
/// </summary>
public class CreateFlightCommandValidator : AbstractValidator<CreateFlightCommand>
{
    public CreateFlightCommandValidator()
    {
        // FlightNumber: required, valid format (e.g., AA1234, BA789)
        RuleFor(x => x.FlightNumber)
            .NotEmpty().WithMessage("Flight number is required")
            .MaximumLength(10).WithMessage("Flight number cannot exceed 10 characters")
            .Matches("^[A-Z]{2}[0-9]{1,4}$").WithMessage("Flight number must be in format: 2 letters followed by 1-4 digits (e.g., AA1234)");

        // AirlineId: required
        RuleFor(x => x.AirlineId)
            .NotEmpty().WithMessage("Airline ID is required");

        // AircraftId: required
        RuleFor(x => x.AircraftId)
            .NotEmpty().WithMessage("Aircraft ID is required");

        // DepartureAirportId: required
        RuleFor(x => x.DepartureAirportId)
            .NotEmpty().WithMessage("Departure airport ID is required");

        // ArrivalAirportId: required, must be different from departure
        RuleFor(x => x.ArrivalAirportId)
            .NotEmpty().WithMessage("Arrival airport ID is required")
            .NotEqual(x => x.DepartureAirportId).WithMessage("Arrival airport must be different from departure airport");

        // ScheduledDepartureTime: required, must be in the future
        RuleFor(x => x.ScheduledDepartureTime)
            .NotEmpty().WithMessage("Scheduled departure time is required")
            .GreaterThan(DateTime.UtcNow).WithMessage("Scheduled departure time must be in the future");

        // ScheduledArrivalTime: required, must be after departure
        RuleFor(x => x.ScheduledArrivalTime)
            .NotEmpty().WithMessage("Scheduled arrival time is required")
            .GreaterThan(x => x.ScheduledDepartureTime).WithMessage("Scheduled arrival time must be after departure time");

        // DepartureTerminal: optional, max 10 characters
        RuleFor(x => x.DepartureTerminal)
            .MaximumLength(10).WithMessage("Departure terminal cannot exceed 10 characters")
            .When(x => !string.IsNullOrEmpty(x.DepartureTerminal));

        // DepartureGate: optional, max 10 characters
        RuleFor(x => x.DepartureGate)
            .MaximumLength(10).WithMessage("Departure gate cannot exceed 10 characters")
            .When(x => !string.IsNullOrEmpty(x.DepartureGate));

        // ArrivalTerminal: optional, max 10 characters
        RuleFor(x => x.ArrivalTerminal)
            .MaximumLength(10).WithMessage("Arrival terminal cannot exceed 10 characters")
            .When(x => !string.IsNullOrEmpty(x.ArrivalTerminal));

        // ArrivalGate: optional, max 10 characters
        RuleFor(x => x.ArrivalGate)
            .MaximumLength(10).WithMessage("Arrival gate cannot exceed 10 characters")
            .When(x => !string.IsNullOrEmpty(x.ArrivalGate));
    }
}

