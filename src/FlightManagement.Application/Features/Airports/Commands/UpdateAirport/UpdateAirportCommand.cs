using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Airports.Commands.UpdateAirport;

/// <summary>
/// Command to update an existing airport.
/// </summary>
public record UpdateAirportCommand(
    Guid Id,
    string Name,
    string City,
    string Country,
    string CountryCode,
    string Timezone,
    decimal Latitude,
    decimal Longitude,
    bool IsActive
) : ICommand;

