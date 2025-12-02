using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Bookings.Commands.ConfirmBooking;

/// <summary>
/// Command to confirm a booking after payment.
/// </summary>
public record ConfirmBookingCommand(
    Guid BookingId,
    string TransactionReference,
    PaymentMethod PaymentMethod,
    decimal Amount,
    string? CardLastFour = null,
    string? CardBrand = null
) : ICommand;

