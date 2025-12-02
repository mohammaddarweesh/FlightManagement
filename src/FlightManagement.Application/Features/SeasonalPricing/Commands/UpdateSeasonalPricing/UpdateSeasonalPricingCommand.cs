using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.SeasonalPricing.Commands.UpdateSeasonalPricing;

/// <summary>
/// Command to update an existing seasonal pricing period.
/// </summary>
public record UpdateSeasonalPricingCommand(
    Guid Id,
    string Name,
    string Description,
    SeasonType SeasonType,
    DateTime StartDate,
    DateTime EndDate,
    decimal AdjustmentPercentage,
    Guid? AirlineId,
    Guid? DepartureAirportId,
    Guid? ArrivalAirportId,
    FlightClass? CabinClass,
    int Priority,
    bool IsActive
) : ICommand;

