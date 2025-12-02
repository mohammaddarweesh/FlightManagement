using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Promotions.Commands.DeactivatePromotion;

/// <summary>
/// Command to deactivate a promotion.
/// </summary>
public record DeactivatePromotionCommand(Guid Id) : ICommand;

