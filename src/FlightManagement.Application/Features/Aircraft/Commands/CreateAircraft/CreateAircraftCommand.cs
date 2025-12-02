using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Aircraft.Commands.CreateAircraft;

/// <summary>
/// Command to create a new aircraft with cabin class configuration.
/// </summary>
public record CreateAircraftCommand(
    Guid AirlineId,
    string Model,
    string Manufacturer,
    string RegistrationNumber,
    List<CabinClassInput> CabinClasses
) : ICommand<Guid>;

/// <summary>
/// Input for cabin class configuration.
/// </summary>
public record CabinClassInput(
    FlightClass CabinClass,
    int RowStart,
    int RowEnd,
    string SeatLayout,
    decimal BasePriceMultiplier = 1.0m
);

