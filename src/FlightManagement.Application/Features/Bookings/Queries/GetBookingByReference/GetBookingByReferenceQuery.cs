using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Features.Bookings.Common;

namespace FlightManagement.Application.Features.Bookings.Queries.GetBookingByReference;

/// <summary>
/// Query to get a booking by its reference (PNR).
/// </summary>
public record GetBookingByReferenceQuery(string BookingReference) : IQuery<BookingDto>;

