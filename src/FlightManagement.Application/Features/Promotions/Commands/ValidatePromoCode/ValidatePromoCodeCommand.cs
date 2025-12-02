using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Application.Common.Messaging;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Features.Promotions.Commands.ValidatePromoCode;

/// <summary>
/// Command to validate a promotion code for a booking.
/// </summary>
public record ValidatePromoCodeCommand(
    string PromoCode,
    Guid CustomerId,
    decimal BookingAmount,
    List<FlightInfo> Flights
) : ICommand<PromoCodeValidationResult>;

public record FlightInfo(
    Guid FlightId,
    Guid AirlineId,
    Guid DepartureAirportId,
    Guid ArrivalAirportId,
    FlightClass CabinClass,
    DateTime DepartureDate
);

public record PromoCodeValidationResult(
    bool IsValid,
    Guid? PromotionId,
    string? PromotionName,
    string? ErrorMessage,
    decimal? EstimatedDiscount,
    string? Currency
);

