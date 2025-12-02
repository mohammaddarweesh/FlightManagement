using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Bookings.Commands.CancelBooking;

/// <summary>
/// Command to cancel a booking.
/// </summary>
public record CancelBookingCommand(
    Guid BookingId,
    string? CancellationReason = null
) : ICommand<CancelBookingResult>;

/// <summary>
/// Result of booking cancellation.
/// </summary>
public record CancelBookingResult(
    bool IsRefundable,
    decimal RefundAmount,
    decimal CancellationFee,
    string Message
);

