using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.Promotions.Queries.GetPromotionById;

public class GetPromotionByIdQueryHandler : IQueryHandler<GetPromotionByIdQuery, PromotionDto>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPromotionByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PromotionDto>> Handle(GetPromotionByIdQuery request, CancellationToken cancellationToken)
    {
        var promotion = await _unitOfWork.Repository<Promotion>()
            .GetByIdAsync(request.Id, cancellationToken);

        if (promotion == null)
        {
            return Result<PromotionDto>.Failure($"Promotion with ID '{request.Id}' not found");
        }

        var dto = new PromotionDto(
            promotion.Id,
            promotion.Code,
            promotion.Name,
            promotion.Description,
            promotion.Type,
            promotion.DiscountType,
            promotion.DiscountValue,
            promotion.MaxDiscountAmount,
            promotion.MinBookingAmount,
            promotion.Currency,
            promotion.ValidFrom,
            promotion.ValidTo,
            promotion.MaxTotalUses,
            promotion.MaxUsesPerCustomer,
            promotion.CurrentUsageCount,
            promotion.ApplicableDays,
            promotion.ApplicableRoutes,
            promotion.ApplicableCabinClasses,
            promotion.ApplicableAirlineIds,
            promotion.FirstTimeCustomersOnly,
            promotion.Status,
            promotion.IsActive,
            promotion.CreatedAt
        );

        return Result<PromotionDto>.Success(dto);
    }
}

