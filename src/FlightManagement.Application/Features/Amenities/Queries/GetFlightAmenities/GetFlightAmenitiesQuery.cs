using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Amenities.Queries.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Amenities.Queries.GetFlightAmenities;

/// <summary>
/// Query to get amenities available for a specific flight.
/// </summary>
public record GetFlightAmenitiesQuery(
    Guid FlightId,
    FlightClass? CabinClass = null
) : IQuery<List<FlightAmenityDto>>;

