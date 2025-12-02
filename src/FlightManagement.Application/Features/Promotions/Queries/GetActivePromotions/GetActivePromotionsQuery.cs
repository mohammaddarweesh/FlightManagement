using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Promotions.Queries.GetActivePromotions;

/// <summary>
/// Query to get currently active promotions visible to customers.
/// </summary>
public record GetActivePromotionsQuery(
    Guid? CustomerId = null
) : IQuery<IEnumerable<ActivePromotionDto>>;

public record ActivePromotionDto(
    Guid Id,
    string Code,
    string Name,
    string? Description,
    DiscountType DiscountType,
    decimal DiscountValue,
    decimal? MaxDiscountAmount,
    decimal? MinBookingAmount,
    string Currency,
    DateTime ValidFrom,
    DateTime ValidTo,
    bool RequiresFirstTimeCustomer
);

