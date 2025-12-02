using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.Policies.Queries.GetBlackoutDates;

public class GetBlackoutDatesQueryHandler : IQueryHandler<GetBlackoutDatesQuery, BlackoutDateListResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBlackoutDatesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BlackoutDateListResult>> Handle(
        GetBlackoutDatesQuery request,
        CancellationToken cancellationToken)
    {
        var baseQuery = _unitOfWork.Repository<BlackoutDate>().Query();

        if (request.IsActive.HasValue)
            baseQuery = baseQuery.Where(b => b.IsActive == request.IsActive.Value);

        if (request.FromDate.HasValue)
            baseQuery = baseQuery.Where(b => b.EndDate >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            baseQuery = baseQuery.Where(b => b.StartDate <= request.ToDate.Value);

        if (request.AirlineId.HasValue)
            baseQuery = baseQuery.Where(b => b.AirlineId == request.AirlineId.Value);

        var totalCount = await baseQuery.CountAsync(cancellationToken);

        var blackoutDates = await baseQuery
            .Include(b => b.Airline)
            .Include(b => b.DepartureAirport)
            .Include(b => b.ArrivalAirport)
            .OrderBy(b => b.StartDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = blackoutDates.Select(b => new BlackoutDateDto(
            b.Id,
            b.Name,
            b.Description,
            b.StartDate,
            b.EndDate,
            b.BlocksBookings,
            b.BlocksPromotions,
            b.AirlineId,
            b.Airline?.Name,
            b.DepartureAirportId,
            b.DepartureAirport?.IataCode,
            b.ArrivalAirportId,
            b.ArrivalAirport?.IataCode,
            b.IsActive
        )).ToList();

        var totalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize);

        return Result<BlackoutDateListResult>.Success(new BlackoutDateListResult(
            items,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        ));
    }
}

