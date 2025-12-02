using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Promotions.Commands.ActivatePromotion;

/// <summary>
/// Command to activate a promotion.
/// </summary>
public record ActivatePromotionCommand(Guid Id) : ICommand;

