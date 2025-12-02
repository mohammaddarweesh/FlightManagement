using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Promotions.Commands.CreatePromotion;

/// <summary>
/// Command to create a new promotion.
/// </summary>
public record CreatePromotionCommand(
    string Code,
    string Name,
    string? Description,
    PromotionType Type,
    DiscountType DiscountType,
    decimal DiscountValue,
    decimal? MaxDiscountAmount,
    decimal? MinBookingAmount,
    string Currency,
    DateTime ValidFrom,
    DateTime ValidTo,
    int? MaxTotalUses,
    int? MaxUsesPerCustomer,
    DayOfWeekFlag ApplicableDays,
    string? ApplicableRoutes,
    string? ApplicableCabinClasses,
    string? ApplicableAirlineIds,
    bool FirstTimeCustomersOnly
) : ICommand<Guid>;

