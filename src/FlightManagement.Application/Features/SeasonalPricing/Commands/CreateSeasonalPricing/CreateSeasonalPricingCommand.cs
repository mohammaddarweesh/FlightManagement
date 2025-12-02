using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.SeasonalPricing.Commands.CreateSeasonalPricing;

/// <summary>
/// Command to create a new seasonal pricing period.
/// </summary>
public record CreateSeasonalPricingCommand(
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
    int Priority = 0,
    bool IsActive = true
) : ICommand<Guid>;

