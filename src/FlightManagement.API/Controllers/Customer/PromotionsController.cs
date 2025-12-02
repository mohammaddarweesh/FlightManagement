using System.Security.Claims;
using FlightManagement.Application.Features.Promotions.Commands.ValidatePromoCode;
using FlightManagement.Application.Features.Promotions.Queries.GetActivePromotions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Customer;

[Authorize]
[Route("api/customer/promotions")]
public class PromotionsController : BaseApiController
{
    /// <summary>
    /// Get all currently active promotions available to the customer.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetActivePromotions()
    {
        var customerId = GetCurrentCustomerId();
        var result = await Mediator.Send(new GetActivePromotionsQuery(customerId));
        return HandleResult(result);
    }

    /// <summary>
    /// Validate a promotion code for a potential booking.
    /// </summary>
    [HttpPost("validate")]
    public async Task<IActionResult> ValidatePromoCode([FromBody] ValidatePromoCodeRequest request)
    {
        var customerId = GetCurrentCustomerId();
        if (!customerId.HasValue)
            return Unauthorized("Customer profile required");

        var flights = request.Flights.Select(f => new FlightInfo(
            f.FlightId,
            f.AirlineId,
            f.DepartureAirportId,
            f.ArrivalAirportId,
            f.CabinClass,
            f.DepartureDate
        )).ToList();

        var command = new ValidatePromoCodeCommand(
            request.PromoCode,
            customerId.Value,
            request.BookingAmount,
            flights
        );

        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    private Guid? GetCurrentCustomerId()
    {
        var customerIdClaim = User.FindFirst("CustomerId")?.Value;
        return Guid.TryParse(customerIdClaim, out var customerId) ? customerId : null;
    }
}

public record ValidatePromoCodeRequest(
    string PromoCode,
    decimal BookingAmount,
    List<FlightInfoRequest> Flights
);

public record FlightInfoRequest(
    Guid FlightId,
    Guid AirlineId,
    Guid DepartureAirportId,
    Guid ArrivalAirportId,
    FlightManagement.Domain.Enums.FlightClass CabinClass,
    DateTime DepartureDate
);

