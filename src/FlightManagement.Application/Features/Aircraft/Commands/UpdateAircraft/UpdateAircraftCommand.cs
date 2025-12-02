using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Aircraft.Commands.UpdateAircraft;

/// <summary>
/// Command to update an existing aircraft.
/// Note: Cabin classes and seats cannot be modified after creation.
/// </summary>
public record UpdateAircraftCommand(
    Guid Id,
    string Model,
    string Manufacturer,
    string RegistrationNumber,
    bool IsActive = true
) : ICommand;

