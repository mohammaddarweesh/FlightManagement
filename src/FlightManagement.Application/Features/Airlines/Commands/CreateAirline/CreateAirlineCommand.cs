using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Airlines.Commands.CreateAirline;

/// <summary>
/// Command to create a new airline.
/// </summary>
public record CreateAirlineCommand(
    string IataCode,
    string IcaoCode,
    string Name,
    string Country,
    string? LogoUrl = null
) : ICommand<Guid>;

