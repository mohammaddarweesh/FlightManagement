using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Policies.Commands.CreateBookingPolicy;

/// <summary>
/// Command to create a new booking policy.
/// </summary>
public record CreateBookingPolicyCommand(
    string Code,
    string Name,
    string? Description,
    PolicyType Type,
    int Value,
    string ErrorMessage,
    Guid? AirlineId,
    Guid? DepartureAirportId,
    Guid? ArrivalAirportId,
    FlightClass? CabinClass,
    int Priority
) : ICommand<Guid>;

