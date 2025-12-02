using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.Reports.Queries.GetBookingStatusSummary;

public class GetBookingStatusSummaryQueryHandler : IQueryHandler<GetBookingStatusSummaryQuery, BookingStatusSummaryResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetBookingStatusSummaryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<BookingStatusSummaryResult>> Handle(
        GetBookingStatusSummaryQuery request,
        CancellationToken cancellationToken)
    {
        var baseQuery = _unitOfWork.Repository<Booking>()
            .Query()
            .Where(b => b.BookingDate >= request.StartDate && b.BookingDate <= request.EndDate);

        if (request.AirlineId.HasValue)
            baseQuery = baseQuery.Where(b => b.Segments.Any(s => s.Flight.AirlineId == request.AirlineId.Value));

        var bookings = await baseQuery
            .Include(b => b.Segments)
                .ThenInclude(s => s.Flight)
            .ToListAsync(cancellationToken);
        var totalBookings = bookings.Count;

        // Status breakdown
        var statusBreakdown = bookings
            .GroupBy(b => b.Status)
            .Select(g => new StatusCount(
                g.Key,
                g.Count(),
                totalBookings > 0 ? Math.Round((decimal)g.Count() / totalBookings * 100, 2) : 0,
                g.Sum(b => b.TotalAmount)
            ))
            .OrderByDescending(s => s.Count)
            .ToList();

        // Daily trend
        var dailyTrend = bookings
            .GroupBy(b => b.BookingDate.Date)
            .Select(g => new DailyStatusSummary(
                g.Key,
                g.Count(b => b.Status == BookingStatus.Confirmed),
                g.Count(b => b.Status == BookingStatus.Cancelled),
                g.Count(b => b.Status == BookingStatus.Pending),
                g.Count(b => b.Status == BookingStatus.NoShow),
                g.Count()
            ))
            .OrderBy(d => d.Date)
            .ToList();

        return Result<BookingStatusSummaryResult>.Success(new BookingStatusSummaryResult(
            request.StartDate,
            request.EndDate,
            totalBookings,
            statusBreakdown,
            dailyTrend
        ));
    }
}

