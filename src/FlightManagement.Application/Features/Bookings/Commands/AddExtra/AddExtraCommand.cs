using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Bookings.Commands.AddExtra;

/// <summary>
/// Command to add an extra service to a booking.
/// </summary>
public record AddExtraCommand(
    Guid BookingId,
    ExtraType ExtraType,
    string Description,
    int Quantity,
    decimal UnitPrice,
    Guid? BookingSegmentId = null,
    Guid? PassengerId = null,
    Guid? FlightAmenityId = null
) : ICommand<AddExtraResult>;

/// <summary>
/// Result of adding an extra.
/// </summary>
public record AddExtraResult(
    Guid BookingExtraId,
    decimal TotalPrice,
    decimal NewBookingTotal
);

