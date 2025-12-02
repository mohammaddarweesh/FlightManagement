using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Promotions.Queries.GetPromotionById;

public record GetPromotionByIdQuery(Guid Id) : IQuery<PromotionDto>;

public record PromotionDto(
    Guid Id,
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
    int CurrentUsageCount,
    DayOfWeekFlag ApplicableDays,
    string? ApplicableRoutes,
    string? ApplicableCabinClasses,
    string? ApplicableAirlineIds,
    bool FirstTimeCustomersOnly,
    PromotionStatus Status,
    bool IsActive,
    DateTime CreatedAt
);

