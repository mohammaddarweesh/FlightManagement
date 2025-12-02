using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.SeasonalPricing.Commands.DeleteSeasonalPricing;

/// <summary>
/// Command to delete a seasonal pricing period.
/// </summary>
public record DeleteSeasonalPricingCommand(Guid Id) : ICommand;

