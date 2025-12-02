using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Airlines.Commands.UpdateAirline;

/// <summary>
/// Command to update an existing airline.
/// </summary>
public record UpdateAirlineCommand(
    Guid Id,
    string IataCode,
    string IcaoCode,
    string Name,
    string Country,
    string? LogoUrl = null,
    bool IsActive = true
) : ICommand;

