using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Promotions.Queries.GetPromotionById;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.Promotions.Queries.GetPromotions;

public class GetPromotionsQueryHandler : IQueryHandler<GetPromotionsQuery, PromotionListResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPromotionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PromotionListResult>> Handle(GetPromotionsQuery request, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<Promotion>().Query();

        if (request.Status.HasValue)
            query = query.Where(p => p.Status == request.Status.Value);

        if (request.Type.HasValue)
            query = query.Where(p => p.Type == request.Type.Value);

        if (request.IsActive.HasValue)
            query = query.Where(p => p.IsActive == request.IsActive.Value);

        var totalCount = await query.CountAsync(cancellationToken);

        var promotions = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = promotions.Select(p => new PromotionDto(
            p.Id,
            p.Code,
            p.Name,
            p.Description,
            p.Type,
            p.DiscountType,
            p.DiscountValue,
            p.MaxDiscountAmount,
            p.MinBookingAmount,
            p.Currency,
            p.ValidFrom,
            p.ValidTo,
            p.MaxTotalUses,
            p.MaxUsesPerCustomer,
            p.CurrentUsageCount,
            p.ApplicableDays,
            p.ApplicableRoutes,
            p.ApplicableCabinClasses,
            p.ApplicableAirlineIds,
            p.FirstTimeCustomersOnly,
            p.Status,
            p.IsActive,
            p.CreatedAt
        )).ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return Result<PromotionListResult>.Success(new PromotionListResult(
            items,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        ));
    }
}

