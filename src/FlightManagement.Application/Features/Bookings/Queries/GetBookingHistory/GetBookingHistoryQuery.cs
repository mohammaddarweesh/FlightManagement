using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Bookings.Queries.GetBookingHistory;

/// <summary>
/// Query to get the history of a booking.
/// </summary>
public record GetBookingHistoryQuery(Guid BookingId) : IQuery<List<BookingHistoryDto>>;

/// <summary>
/// Booking history entry DTO.
/// </summary>
public record BookingHistoryDto(
    Guid Id,
    BookingAction Action,
    string Description,
    string? OldValues,
    string? NewValues,
    ActorType PerformedByType,
    Guid? PerformedById,
    DateTime PerformedAt,
    string? IpAddress
);

