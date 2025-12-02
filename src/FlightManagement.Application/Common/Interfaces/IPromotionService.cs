using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Application.Common.Interfaces;

/// <summary>
/// Service for validating and applying promotions/discount codes.
/// </summary>
public interface IPromotionService
{
    /// <summary>
    /// Validates a promotion code for a booking.
    /// </summary>
    Task<PromotionValidationResult> ValidatePromotionAsync(
        string promoCode,
        Guid customerId,
        decimal bookingAmount,
        List<PromotionFlightInfo> flights,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Calculates the discount for a promotion.
    /// </summary>
    Task<PromotionDiscountResult> CalculateDiscountAsync(
        Guid promotionId,
        decimal bookingAmount,
        int passengerCount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Records the usage of a promotion for a booking.
    /// </summary>
    Task RecordPromotionUsageAsync(
        Guid promotionId,
        Guid customerId,
        Guid bookingId,
        decimal discountAmount,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the usage count for a customer on a promotion.
    /// </summary>
    Task<int> GetCustomerUsageCountAsync(
        Guid promotionId,
        Guid customerId,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Flight information for promotion validation.
/// </summary>
public record PromotionFlightInfo(
    Guid FlightId,
    Guid AirlineId,
    Guid DepartureAirportId,
    Guid ArrivalAirportId,
    FlightClass CabinClass,
    DateTime DepartureDate
);

/// <summary>
/// Result of promotion validation.
/// </summary>
public record PromotionValidationResult(
    bool IsValid,
    Guid? PromotionId,
    string? PromotionName,
    string? ErrorMessage,
    decimal? EstimatedDiscount
);

/// <summary>
/// Result of discount calculation.
/// </summary>
public record PromotionDiscountResult(
    decimal DiscountAmount,
    DiscountType DiscountType,
    decimal DiscountValue,
    decimal? MaxDiscount,
    string Currency
);

