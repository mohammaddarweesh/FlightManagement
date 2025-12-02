using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Bookings.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Bookings.Queries.GetCustomerBookings;

/// <summary>
/// Handler for getting customer bookings.
/// </summary>
public class GetCustomerBookingsQueryHandler : IQueryHandler<GetCustomerBookingsQuery, PagedResult<BookingSimpleDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCustomerBookingsQueryHandler> _logger;

    public GetCustomerBookingsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetCustomerBookingsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<PagedResult<BookingSimpleDto>>> Handle(GetCustomerBookingsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting bookings for customer: {CustomerId}", request.CustomerId);

        var bookingRepo = _unitOfWork.Repository<Booking>();
        var query = bookingRepo.Query()
            .Include(b => b.Customer)
            .Include(b => b.Segments)
                .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f!.DepartureAirport)
            .Include(b => b.Segments)
                .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f!.ArrivalAirport)
            .Include(b => b.Passengers)
            .Where(b => b.CustomerId == request.CustomerId);

        // Apply filters
        if (request.Status.HasValue)
            query = query.Where(b => b.Status == request.Status.Value);

        if (request.FromDate.HasValue)
            query = query.Where(b => b.BookingDate >= request.FromDate.Value);

        if (request.ToDate.HasValue)
            query = query.Where(b => b.BookingDate <= request.ToDate.Value);

        // Get total count
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var bookings = await query
            .OrderByDescending(b => b.BookingDate)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        var items = bookings.Select(b => MapToSimpleDto(b)).ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / request.PageSize);

        return Result<PagedResult<BookingSimpleDto>>.Success(new PagedResult<BookingSimpleDto>(
            items,
            totalCount,
            request.Page,
            request.PageSize,
            totalPages
        ));
    }

    private static BookingSimpleDto MapToSimpleDto(Booking b)
    {
        var firstSegment = b.Segments.OrderBy(s => s.SegmentOrder).FirstOrDefault();
        var lastSegment = b.Segments.OrderByDescending(s => s.SegmentOrder).FirstOrDefault();

        string route = "";
        DateTime departureDate = DateTime.MinValue;

        if (firstSegment?.Flight != null && lastSegment?.Flight != null)
        {
            route = $"{firstSegment.Flight.DepartureAirport?.IataCode} → {lastSegment.Flight.ArrivalAirport?.IataCode}";
            departureDate = firstSegment.Flight.ScheduledDepartureTime;
        }

        return new BookingSimpleDto(
            b.Id,
            b.BookingReference,
            $"{b.Customer?.FirstName} {b.Customer?.LastName}",
            b.Status,
            b.TripType,
            route,
            departureDate,
            b.TotalAmount,
            b.Currency,
            b.PaymentStatus,
            b.Passengers.Count,
            b.BookingDate
        );
    }
}

