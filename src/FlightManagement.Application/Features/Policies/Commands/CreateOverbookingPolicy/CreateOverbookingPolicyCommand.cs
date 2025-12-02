using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Policies.Commands.CreateOverbookingPolicy;

/// <summary>
/// Command to create a new overbooking policy.
/// </summary>
public record CreateOverbookingPolicyCommand(
    string Name,
    string? Description,
    Guid AirlineId,
    decimal MaxOverbookingPercentage,
    int? MaxOverbookedSeats,
    Guid? DepartureAirportId,
    Guid? ArrivalAirportId,
    FlightClass? CabinClass,
    int Priority
) : ICommand<Guid>;

