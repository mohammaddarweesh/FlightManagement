using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Amenities.Queries.GetFlightAmenities;
using FlightManagement.Application.Features.Flights.Queries.SearchFlights;
using FlightManagement.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Customer;

/// <summary>
/// Customer controller for searching and viewing flights.
/// Accessible by both Customers and Admins.
/// </summary>
[Route("api/customer/flights")]
[RequireCustomer]
public class FlightSearchController : BaseApiController
{
    /// <summary>
    /// Search for available flights.
    /// </summary>
    /// <param name="departureAirportCode">IATA code of departure airport (e.g., "JFK")</param>
    /// <param name="arrivalAirportCode">IATA code of arrival airport (e.g., "LAX")</param>
    /// <param name="departureDate">Date of departure</param>
    /// <param name="cabinClass">Preferred cabin class</param>
    /// <param name="passengers">Number of passengers (for seat availability check)</param>
    [HttpGet("search")]
    public async Task<IActionResult> Search(
        [FromQuery] string? departureAirportCode = null,
        [FromQuery] string? arrivalAirportCode = null,
        [FromQuery] DateTime? departureDate = null,
        [FromQuery] FlightClass? cabinClass = null,
        [FromQuery] int? passengers = null)
    {
        var query = new SearchFlightsQuery(
            departureAirportCode,
            arrivalAirportCode,
            departureDate,
            cabinClass,
            passengers
        );
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    /// <summary>
    /// Get amenities available for a specific flight.
    /// </summary>
    /// <param name="flightId">Flight ID</param>
    /// <param name="cabinClass">Filter by cabin class</param>
    [HttpGet("{flightId:guid}/amenities")]
    public async Task<IActionResult> GetFlightAmenities(
        Guid flightId,
        [FromQuery] FlightClass? cabinClass = null)
    {
        var result = await Mediator.Send(new GetFlightAmenitiesQuery(flightId, cabinClass));
        return HandleResult(result);
    }
}

