using FlightManagement.Application.Common.Messaging;

namespace FlightManagement.Application.Features.Bookings.Commands.UpdateBooking;

/// <summary>
/// Command to update booking contact information and special requests.
/// </summary>
public record UpdateBookingCommand(
    Guid BookingId,
    string? ContactEmail = null,
    string? ContactPhone = null,
    string? SpecialRequests = null
) : ICommand;

