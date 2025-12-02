using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Bookings.Common;

namespace FlightManagement.Application.Features.Bookings.Queries.GetBookingById;

/// <summary>
/// Query to get a booking by its ID.
/// </summary>
public record GetBookingByIdQuery(Guid BookingId) : IQuery<BookingDto>;

