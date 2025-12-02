using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Promotions.Queries.GetPromotionUsage;

public record GetPromotionUsageQuery(
    Guid PromotionId,
    int Page = 1,
    int PageSize = 20
) : IQuery<PromotionUsageListResult>;

public record PromotionUsageListResult(
    List<PromotionUsageDto> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

public record PromotionUsageDto(
    Guid Id,
    Guid CustomerId,
    string CustomerName,
    string CustomerEmail,
    Guid BookingId,
    string BookingReference,
    decimal DiscountAmount,
    string Currency,
    DateTime UsedAt
);

