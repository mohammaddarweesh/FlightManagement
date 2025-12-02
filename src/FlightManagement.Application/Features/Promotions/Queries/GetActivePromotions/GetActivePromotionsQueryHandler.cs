using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.Promotions.Queries.GetActivePromotions;

public class GetActivePromotionsQueryHandler : IQueryHandler<GetActivePromotionsQuery, IEnumerable<ActivePromotionDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetActivePromotionsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<ActivePromotionDto>>> Handle(
        GetActivePromotionsQuery request,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var query = _unitOfWork.Repository<Promotion>()
            .Query()
            .Where(p => p.Status == PromotionStatus.Active)
            .Where(p => p.ValidFrom <= now && p.ValidTo >= now)
            .Where(p => !p.MaxTotalUses.HasValue || p.CurrentUsageCount < p.MaxTotalUses.Value);

        // If customer ID is provided, filter out first-time-only promotions for returning customers
        if (request.CustomerId.HasValue)
        {
            var hasBookings = await _unitOfWork.Repository<Booking>()
                .Query()
                .AnyAsync(b => b.CustomerId == request.CustomerId.Value 
                    && b.Status != BookingStatus.Cancelled, cancellationToken);

            if (hasBookings)
            {
                // Filter out first-time customer only promotions
                query = query.Where(p => !p.FirstTimeCustomersOnly);
            }
        }

        var promotions = await query
            .OrderByDescending(p => p.DiscountValue)
            .ToListAsync(cancellationToken);

        var dtos = promotions.Select(p => new ActivePromotionDto(
            p.Id,
            p.Code,
            p.Name,
            p.Description,
            p.DiscountType,
            p.DiscountValue,
            p.MaxDiscountAmount,
            p.MinBookingAmount,
            p.Currency,
            p.ValidFrom,
            p.ValidTo,
            p.FirstTimeCustomersOnly
        ));

        return Result<IEnumerable<ActivePromotionDto>>.Success(dtos);
    }
}

