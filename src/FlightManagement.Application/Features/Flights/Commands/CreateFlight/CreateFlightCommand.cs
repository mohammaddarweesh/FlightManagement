using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Flights.Commands.CreateFlight;

/// <summary>
/// Command to create a new flight.
/// </summary>
public record CreateFlightCommand(
    string FlightNumber,
    Guid AirlineId,
    Guid AircraftId,
    Guid DepartureAirportId,
    Guid ArrivalAirportId,
    DateTime ScheduledDepartureTime,
    DateTime ScheduledArrivalTime,
    string? DepartureTerminal = null,
    string? DepartureGate = null,
    string? ArrivalTerminal = null,
    string? ArrivalGate = null
) : ICommand<Guid>;

