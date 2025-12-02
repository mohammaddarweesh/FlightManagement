using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Amenities.Commands.CreateAmenity;

/// <summary>
/// Command to create a new amenity.
/// </summary>
public record CreateAmenityCommand(
    string Code,
    string Name,
    string Description,
    AmenityCategory Category,
    string? IconUrl = null
) : ICommand<Guid>;

