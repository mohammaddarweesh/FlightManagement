using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.Promotions.Queries.GetPromotionUsage;

public class GetPromotionUsageQueryHandler : IQueryHandler<GetPromotionUsageQuery, PromotionUsageListResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPromotionUsageQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PromotionUsageListResult>> Handle(
        GetPromotionUsageQuery request,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Repository<PromotionUsage>()
            .Query()
            .Include(u => u.Customer)
                .ThenInclude(c => c.User)
            .Include(u => u.Booking)
            .Where(u => u.PromotionId == request.PromotionId);

        var totalCount = await query.CountAsync(cancellationToken);

        var usages = await query
            .OrderByDescending(u => u.UsedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = usages.Select(u => new PromotionUsageDto(
            u.Id,
            u.CustomerId,
            $"{u.Customer.FirstName} {u.Customer.LastName}",
            u.Customer.User.Email,
            u.BookingId,
            u.Booking.BookingReference,
            u.DiscountAmount,
            u.Currency,
            u.UsedAt
        )).ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return Result<PromotionUsageListResult>.Success(new PromotionUsageListResult(
            items,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        ));
    }
}

