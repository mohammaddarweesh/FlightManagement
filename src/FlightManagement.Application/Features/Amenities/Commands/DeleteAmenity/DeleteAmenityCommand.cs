using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Amenities.Commands.DeleteAmenity;

/// <summary>
/// Command to delete (soft delete) an amenity.
/// </summary>
public record DeleteAmenityCommand(Guid Id) : ICommand;

