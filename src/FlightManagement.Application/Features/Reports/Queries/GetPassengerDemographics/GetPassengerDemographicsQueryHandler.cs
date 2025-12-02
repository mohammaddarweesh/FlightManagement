using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Application.Features.Reports.Queries.GetPassengerDemographics;

public class GetPassengerDemographicsQueryHandler : IQueryHandler<GetPassengerDemographicsQuery, PassengerDemographicsResult>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPassengerDemographicsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<PassengerDemographicsResult>> Handle(
        GetPassengerDemographicsQuery request,
        CancellationToken cancellationToken)
    {
        var baseQuery = _unitOfWork.Repository<Booking>()
            .Query()
            .Where(b => b.BookingDate >= request.StartDate && b.BookingDate <= request.EndDate)
            .Where(b => b.Status != BookingStatus.Cancelled);

        if (request.AirlineId.HasValue)
            baseQuery = baseQuery.Where(b => b.Segments.Any(s => s.Flight.AirlineId == request.AirlineId.Value));

        var bookings = await baseQuery
            .Include(b => b.Customer)
                .ThenInclude(c => c.User)
            .Include(b => b.Passengers)
            .Include(b => b.Segments)
                .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f.DepartureAirport)
            .Include(b => b.Segments)
                .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f.ArrivalAirport)
            .ToListAsync(cancellationToken);

        var allPassengers = bookings.SelectMany(b => b.Passengers).ToList();
        var totalPassengers = allPassengers.Count;
        var uniqueCustomers = bookings.Select(b => b.CustomerId).Distinct().Count();
        var avgPassengersPerBooking = bookings.Count > 0 ? (decimal)totalPassengers / bookings.Count : 0;

        // Passenger type breakdown
        var passengerTypes = allPassengers
            .GroupBy(p => p.PassengerType)
            .Select(g => new PassengerTypeBreakdown(
                g.Key,
                g.Count(),
                totalPassengers > 0 ? Math.Round((decimal)g.Count() / totalPassengers * 100, 2) : 0
            ))
            .OrderByDescending(p => p.Count)
            .ToList();

        // Cabin class preferences
        var cabinClassPrefs = bookings
            .SelectMany(b => b.Segments.Select(s => new { Segment = s, Booking = b }))
            .GroupBy(x => x.Segment.CabinClass)
            .Select(g => new CabinClassPreference(
                g.Key,
                g.Select(x => x.Booking.Id).Distinct().Count(),
                bookings.Count > 0 ? Math.Round((decimal)g.Select(x => x.Booking.Id).Distinct().Count() / bookings.Count * 100, 2) : 0,
                g.Any() ? g.Average(x => x.Booking.TotalAmount) : 0
            ))
            .OrderByDescending(c => c.BookingCount)
            .ToList();

        // Booking patterns
        var bookingPatterns = GetBookingPatterns(bookings);

        // Top routes
        var topRoutes = bookings
            .SelectMany(b => b.Segments.Select(s => new { Segment = s, Booking = b }))
            .GroupBy(x => new { 
                Dep = x.Segment.Flight.DepartureAirport.IataCode, 
                Arr = x.Segment.Flight.ArrivalAirport.IataCode 
            })
            .Select(g => new TopRoute(
                g.Key.Dep,
                g.Key.Arr,
                g.Select(x => x.Booking.Id).Distinct().Count(),
                g.Sum(x => x.Booking.Passengers.Count),
                g.Sum(x => x.Booking.TotalAmount / x.Booking.Segments.Count)
            ))
            .OrderByDescending(r => r.BookingCount)
            .Take(10)
            .ToList();

        // Frequent customers
        var frequentCustomers = bookings
            .GroupBy(b => b.Customer)
            .Select(g => new FrequentCustomer(
                g.Key.Id,
                $"{g.Key.FirstName} {g.Key.LastName}",
                g.Key.User.Email,
                g.Count(),
                g.Sum(b => b.Segments.Count),
                g.Sum(b => b.TotalAmount)
            ))
            .OrderByDescending(c => c.BookingCount)
            .Take(10)
            .ToList();

        return Result<PassengerDemographicsResult>.Success(new PassengerDemographicsResult(
            request.StartDate,
            request.EndDate,
            totalPassengers,
            uniqueCustomers,
            Math.Round(avgPassengersPerBooking, 2),
            passengerTypes,
            cabinClassPrefs,
            bookingPatterns,
            topRoutes,
            frequentCustomers
        ));
    }

    private List<BookingPatternInsight> GetBookingPatterns(List<Booking> bookings)
    {
        var patterns = new List<BookingPatternInsight>();
        var total = bookings.Count;
        if (total == 0) return patterns;

        // Weekend vs weekday bookings
        var weekendBookings = bookings.Count(b => 
            b.BookingDate.DayOfWeek == DayOfWeek.Saturday || 
            b.BookingDate.DayOfWeek == DayOfWeek.Sunday);
        patterns.Add(new BookingPatternInsight(
            "WeekendBookings",
            "Bookings made on weekends",
            weekendBookings,
            Math.Round((decimal)weekendBookings / total * 100, 2)
        ));

        // Same-day bookings
        var sameDayBookings = bookings.Count(b =>
            b.Segments.Any(s => s.Flight.ScheduledDepartureTime.Date == b.BookingDate.Date));
        patterns.Add(new BookingPatternInsight(
            "SameDayBookings",
            "Bookings made on the day of travel",
            sameDayBookings,
            Math.Round((decimal)sameDayBookings / total * 100, 2)
        ));

        // Multi-segment bookings
        var multiSegment = bookings.Count(b => b.Segments.Count > 1);
        patterns.Add(new BookingPatternInsight(
            "MultiSegmentBookings",
            "Bookings with multiple flight segments",
            multiSegment,
            Math.Round((decimal)multiSegment / total * 100, 2)
        ));

        // Group bookings (3+ passengers)
        var groupBookings = bookings.Count(b => b.Passengers.Count >= 3);
        patterns.Add(new BookingPatternInsight(
            "GroupBookings",
            "Bookings with 3 or more passengers",
            groupBookings,
            Math.Round((decimal)groupBookings / total * 100, 2)
        ));

        return patterns;
    }
}

