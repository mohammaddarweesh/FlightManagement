using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.Reports.Queries.GetRevenueReport;

public class GetRevenueReportQueryHandler : IQueryHandler<GetRevenueReportQuery, RevenueReportResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRevenueReportQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<RevenueReportResult>> Handle(
        GetRevenueReportQuery request,
        CancellationToken cancellationToken)
    {
        var baseQuery = _unitOfWork.Repository<Booking>()
            .Query()
            .Where(b => b.BookingDate >= request.StartDate && b.BookingDate <= request.EndDate)
            .Where(b => b.Status != BookingStatus.Cancelled);

        if (request.AirlineId.HasValue)
            baseQuery = baseQuery.Where(b => b.Segments.Any(s => s.Flight.AirlineId == request.AirlineId.Value));

        if (request.DepartureAirportId.HasValue)
            baseQuery = baseQuery.Where(b => b.Segments.Any(s => s.Flight.DepartureAirportId == request.DepartureAirportId.Value));

        if (request.ArrivalAirportId.HasValue)
            baseQuery = baseQuery.Where(b => b.Segments.Any(s => s.Flight.ArrivalAirportId == request.ArrivalAirportId.Value));

        if (request.CabinClass.HasValue)
            baseQuery = baseQuery.Where(b => b.Segments.Any(s => s.CabinClass == request.CabinClass.Value));

        var bookings = await baseQuery
            .Include(b => b.Segments)
                .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f.Airline)
            .Include(b => b.Segments)
                .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f.DepartureAirport)
            .Include(b => b.Segments)
                .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f.ArrivalAirport)
            .Include(b => b.Passengers)
            .ToListAsync(cancellationToken);

        var totalRevenue = bookings.Sum(b => b.TotalAmount);
        var totalRefunds = bookings.Sum(b => b.RefundAmount ?? 0);
        var netRevenue = totalRevenue - totalRefunds;
        var totalPassengers = bookings.Sum(b => b.Passengers.Count);
        var avgBookingValue = bookings.Count > 0 ? totalRevenue / bookings.Count : 0;

        var breakdown = GetBreakdown(bookings, request.GroupBy);

        return Result<RevenueReportResult>.Success(new RevenueReportResult(
            request.StartDate,
            request.EndDate,
            totalRevenue,
            totalRefunds,
            netRevenue,
            bookings.Count,
            totalPassengers,
            avgBookingValue,
            "USD",
            breakdown
        ));
    }

    private List<RevenueBreakdown> GetBreakdown(List<Booking> bookings, ReportGroupBy groupBy)
    {
        return groupBy switch
        {
            ReportGroupBy.Day => bookings
                .GroupBy(b => b.BookingDate.Date)
                .Select(g => new RevenueBreakdown(
                    g.Key.ToString("yyyy-MM-dd"),
                    g.Key.ToString("MMM dd, yyyy"),
                    g.Sum(b => b.TotalAmount),
                    g.Sum(b => b.RefundAmount ?? 0),
                    g.Sum(b => b.TotalAmount - (b.RefundAmount ?? 0)),
                    g.Count(),
                    g.Sum(b => b.Passengers.Count)
                ))
                .OrderBy(r => r.GroupKey)
                .ToList(),

            ReportGroupBy.Month => bookings
                .GroupBy(b => new { b.BookingDate.Year, b.BookingDate.Month })
                .Select(g => new RevenueBreakdown(
                    $"{g.Key.Year}-{g.Key.Month:D2}",
                    new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                    g.Sum(b => b.TotalAmount),
                    g.Sum(b => b.RefundAmount ?? 0),
                    g.Sum(b => b.TotalAmount - (b.RefundAmount ?? 0)),
                    g.Count(),
                    g.Sum(b => b.Passengers.Count)
                ))
                .OrderBy(r => r.GroupKey)
                .ToList(),

            ReportGroupBy.CabinClass => bookings
                .SelectMany(b => b.Segments.Select(s => new { Booking = b, s.CabinClass }))
                .GroupBy(x => x.CabinClass)
                .Select(g => new RevenueBreakdown(
                    g.Key.ToString(),
                    g.Key.ToString(),
                    g.Sum(x => x.Booking.TotalAmount / x.Booking.Segments.Count),
                    g.Sum(x => (x.Booking.RefundAmount ?? 0) / x.Booking.Segments.Count),
                    g.Sum(x => (x.Booking.TotalAmount - (x.Booking.RefundAmount ?? 0)) / x.Booking.Segments.Count),
                    g.Select(x => x.Booking.Id).Distinct().Count(),
                    g.Sum(x => x.Booking.Passengers.Count)
                ))
                .ToList(),

            _ => new List<RevenueBreakdown>()
        };
    }
}

