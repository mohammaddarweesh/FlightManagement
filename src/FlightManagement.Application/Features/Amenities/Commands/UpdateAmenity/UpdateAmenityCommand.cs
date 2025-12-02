using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Amenities.Commands.UpdateAmenity;

/// <summary>
/// Command to update an existing amenity.
/// </summary>
public record UpdateAmenityCommand(
    Guid Id,
    string Code,
    string Name,
    string Description,
    AmenityCategory Category,
    string? IconUrl = null,
    bool IsActive = true
) : ICommand;

