using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Promotions.Queries.GetPromotionById;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Promotions.Queries.GetPromotions;

public record GetPromotionsQuery(
    PromotionStatus? Status = null,
    PromotionType? Type = null,
    bool? IsActive = null,
    int Page = 1,
    int PageSize = 20
) : IQuery<PromotionListResult>;

public record PromotionListResult(
    List<PromotionDto> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

