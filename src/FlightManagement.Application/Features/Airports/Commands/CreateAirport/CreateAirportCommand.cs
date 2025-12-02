using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Airports.Commands.CreateAirport;

/// <summary>
/// Command to create a new airport.
/// </summary>
public record CreateAirportCommand(
    string IataCode,
    string IcaoCode,
    string Name,
    string City,
    string Country,
    string CountryCode,
    string Timezone,
    decimal Latitude,
    decimal Longitude
) : ICommand<Guid>;

