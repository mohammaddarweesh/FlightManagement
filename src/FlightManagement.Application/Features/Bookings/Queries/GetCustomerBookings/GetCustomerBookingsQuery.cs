using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Bookings.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Bookings.Queries.GetCustomerBookings;

/// <summary>
/// Query to get all bookings for a customer.
/// </summary>
public record GetCustomerBookingsQuery(
    Guid CustomerId,
    BookingStatus? Status = null,
    DateTime? FromDate = null,
    DateTime? ToDate = null,
    int Page = 1,
    int PageSize = 10
) : IQuery<PagedResult<BookingSimpleDto>>;

/// <summary>
/// Paged result wrapper.
/// </summary>
public record PagedResult<T>(
    List<T> Items,
    int TotalCount,
    int Page,
    int PageSize,
    int TotalPages
);

