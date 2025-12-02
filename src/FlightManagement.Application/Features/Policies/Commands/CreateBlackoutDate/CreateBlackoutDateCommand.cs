using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Policies.Commands.CreateBlackoutDate;

/// <summary>
/// Command to create a new blackout date period.
/// </summary>
public record CreateBlackoutDateCommand(
    string Name,
    string? Description,
    DateTime StartDate,
    DateTime EndDate,
    bool BlocksBookings,
    bool BlocksPromotions,
    Guid? AirlineId,
    Guid? DepartureAirportId,
    Guid? ArrivalAirportId
) : ICommand<Guid>;

