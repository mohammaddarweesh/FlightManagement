using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Promotions.Commands.UpdatePromotion;

/// <summary>
/// Command to update an existing promotion.
/// </summary>
public record UpdatePromotionCommand(
    Guid Id,
    string Name,
    string? Description,
    decimal DiscountValue,
    decimal? MaxDiscountAmount,
    decimal? MinBookingAmount,
    DateTime ValidFrom,
    DateTime ValidTo,
    int? MaxTotalUses,
    int? MaxUsesPerCustomer,
    DayOfWeekFlag ApplicableDays,
    string? ApplicableRoutes,
    string? ApplicableCabinClasses,
    string? ApplicableAirlineIds,
    bool FirstTimeCustomersOnly
) : ICommand;

