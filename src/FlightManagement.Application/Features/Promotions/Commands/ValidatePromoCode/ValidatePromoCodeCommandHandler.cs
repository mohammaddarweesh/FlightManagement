using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Promotions.Commands.ValidatePromoCode;

public class ValidatePromoCodeCommandHandler : ICommandHandler<ValidatePromoCodeCommand, PromoCodeValidationResult>
{
    private readonly IPromotionService _promotionService;
    private readonly ILogger<ValidatePromoCodeCommandHandler> _logger;

    public ValidatePromoCodeCommandHandler(
        IPromotionService promotionService,
        ILogger<ValidatePromoCodeCommandHandler> logger)
    {
        _promotionService = promotionService;
        _logger = logger;
    }

    public async Task<Result<PromoCodeValidationResult>> Handle(
        ValidatePromoCodeCommand request,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Validating promo code: {PromoCode} for customer: {CustomerId}",
            request.PromoCode, request.CustomerId);

        var flightInfos = request.Flights.Select(f => new PromotionFlightInfo(
            f.FlightId,
            f.AirlineId,
            f.DepartureAirportId,
            f.ArrivalAirportId,
            f.CabinClass,
            f.DepartureDate
        )).ToList();

        var validationResult = await _promotionService.ValidatePromotionAsync(
            request.PromoCode,
            request.CustomerId,
            request.BookingAmount,
            flightInfos,
            cancellationToken);

        var result = new PromoCodeValidationResult(
            validationResult.IsValid,
            validationResult.PromotionId,
            validationResult.PromotionName,
            validationResult.ErrorMessage,
            validationResult.EstimatedDiscount,
            validationResult.IsValid ? "USD" : null
        );

        if (validationResult.IsValid)
        {
            _logger.LogInformation("Promo code {PromoCode} validated successfully. Discount: {Discount}",
                request.PromoCode, validationResult.EstimatedDiscount);
        }
        else
        {
            _logger.LogWarning("Promo code {PromoCode} validation failed: {Error}",
                request.PromoCode, validationResult.ErrorMessage);
        }

        return Result<PromoCodeValidationResult>.Success(result);
    }
}

