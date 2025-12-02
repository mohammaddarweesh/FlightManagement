using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Bookings.Commands.AddExtra;
using FlightManagement.Application.Features.Bookings.Commands.CancelBooking;
using FlightManagement.Application.Features.Bookings.Commands.ConfirmBooking;
using FlightManagement.Application.Features.Bookings.Commands.CreateBooking;
using FlightManagement.Application.Features.Bookings.Commands.SelectSeat;
using FlightManagement.Application.Features.Bookings.Commands.UpdateBooking;
using FlightManagement.Application.Features.Bookings.Commands.UpdatePassenger;
using FlightManagement.Application.Features.Bookings.Queries.CalculateBookingPrice;
using FlightManagement.Application.Features.Bookings.Queries.GetAvailableSeats;
using FlightManagement.Application.Features.Bookings.Queries.GetBookingById;
using FlightManagement.Application.Features.Bookings.Queries.GetBookingByReference;
using FlightManagement.Application.Features.Bookings.Queries.GetBookingHistory;
using FlightManagement.Application.Features.Bookings.Queries.GetCustomerBookings;
using FlightManagement.Application.Features.Bookings.Queries.SearchAvailableFlights;
using FlightManagement.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers.Customer;

/// <summary>
/// Customer controller for managing bookings.
/// </summary>
[Route("api/customer/bookings")]
[RequireCustomer]
public class BookingsController : BaseApiController
{
    /// <summary>
    /// Search for available flights.
    /// </summary>
    [HttpGet("search-flights")]
    public async Task<IActionResult> SearchFlights(
        [FromQuery] string departureAirportCode,
        [FromQuery] string arrivalAirportCode,
        [FromQuery] DateTime departureDate,
        [FromQuery] FlightClass? cabinClass = null,
        [FromQuery] int passengerCount = 1,
        [FromQuery] DateTime? returnDate = null)
    {
        var query = new SearchAvailableFlightsQuery(
            departureAirportCode, arrivalAirportCode, departureDate,
            cabinClass, passengerCount, returnDate);
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    /// <summary>
    /// Calculate price for a booking.
    /// </summary>
    [HttpPost("calculate-price")]
    public async Task<IActionResult> CalculatePrice([FromBody] CalculateBookingPriceQuery query)
    {
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    /// <summary>
    /// Get available seats for a flight.
    /// </summary>
    [HttpGet("flights/{flightId:guid}/seats")]
    public async Task<IActionResult> GetAvailableSeats(Guid flightId, [FromQuery] FlightClass? cabinClass = null)
    {
        var result = await Mediator.Send(new GetAvailableSeatsQuery(flightId, cabinClass));
        return HandleResult(result);
    }

    /// <summary>
    /// Create a new booking.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBookingCommand command)
    {
        var result = await Mediator.Send(command);
        return HandleCreatedResult(result, nameof(GetById), new { id = result.Data?.BookingId });
    }

    /// <summary>
    /// Get booking by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await Mediator.Send(new GetBookingByIdQuery(id));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Get booking by reference (PNR).
    /// </summary>
    [HttpGet("reference/{reference}")]
    public async Task<IActionResult> GetByReference(string reference)
    {
        var result = await Mediator.Send(new GetBookingByReferenceQuery(reference));
        return HandleNotFoundResult(result);
    }

    /// <summary>
    /// Get all bookings for a customer.
    /// </summary>
    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetCustomerBookings(
        Guid customerId,
        [FromQuery] BookingStatus? status = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetCustomerBookingsQuery(customerId, status, fromDate, toDate, page, pageSize);
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }

    /// <summary>
    /// Get booking history.
    /// </summary>
    [HttpGet("{id:guid}/history")]
    public async Task<IActionResult> GetHistory(Guid id)
    {
        var result = await Mediator.Send(new GetBookingHistoryQuery(id));
        return HandleResult(result);
    }

    /// <summary>
    /// Update booking contact information.
    /// </summary>
    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBookingRequest request)
    {
        var command = new UpdateBookingCommand(id, request.ContactEmail, request.ContactPhone, request.SpecialRequests);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Confirm booking with payment.
    /// </summary>
    [HttpPost("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm(Guid id, [FromBody] ConfirmBookingRequest request)
    {
        var command = new ConfirmBookingCommand(
            id, request.TransactionReference, request.PaymentMethod,
            request.Amount, request.CardLastFour, request.CardBrand);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Cancel a booking.
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelBookingRequest? request = null)
    {
        var command = new CancelBookingCommand(id, request?.CancellationReason);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Select a seat for a passenger.
    /// </summary>
    [HttpPost("{id:guid}/seats")]
    public async Task<IActionResult> SelectSeat(Guid id, [FromBody] SelectSeatRequest request)
    {
        var command = new SelectSeatCommand(id, request.PassengerId, request.BookingSegmentId, request.FlightSeatId);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Add an extra service to a booking.
    /// </summary>
    [HttpPost("{id:guid}/extras")]
    public async Task<IActionResult> AddExtra(Guid id, [FromBody] AddExtraRequest request)
    {
        var command = new AddExtraCommand(
            id, request.ExtraType, request.Description, request.Quantity,
            request.UnitPrice, request.BookingSegmentId, request.PassengerId, request.FlightAmenityId);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    /// <summary>
    /// Update passenger information.
    /// </summary>
    [HttpPatch("{id:guid}/passengers/{passengerId:guid}")]
    public async Task<IActionResult> UpdatePassenger(Guid id, Guid passengerId, [FromBody] UpdatePassengerRequest request)
    {
        var command = new UpdatePassengerCommand(
            id, passengerId, request.Title, request.FirstName, request.MiddleName, request.LastName,
            request.PassportNumber, request.PassportIssuingCountry, request.PassportExpiryDate,
            request.Email, request.Phone, request.MealPreference, request.SpecialAssistance,
            request.FrequentFlyerNumber, request.FrequentFlyerAirlineId);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}

#region Request DTOs

public record UpdateBookingRequest(
    string? ContactEmail = null,
    string? ContactPhone = null,
    string? SpecialRequests = null
);

public record ConfirmBookingRequest(
    string TransactionReference,
    PaymentMethod PaymentMethod,
    decimal Amount,
    string? CardLastFour = null,
    string? CardBrand = null
);

public record CancelBookingRequest(
    string? CancellationReason = null
);

public record SelectSeatRequest(
    Guid PassengerId,
    Guid BookingSegmentId,
    Guid FlightSeatId
);

public record AddExtraRequest(
    ExtraType ExtraType,
    string Description,
    int Quantity,
    decimal UnitPrice,
    Guid? BookingSegmentId = null,
    Guid? PassengerId = null,
    Guid? FlightAmenityId = null
);

public record UpdatePassengerRequest(
    string? Title = null,
    string? FirstName = null,
    string? MiddleName = null,
    string? LastName = null,
    string? PassportNumber = null,
    string? PassportIssuingCountry = null,
    DateTime? PassportExpiryDate = null,
    string? Email = null,
    string? Phone = null,
    MealPreference? MealPreference = null,
    string? SpecialAssistance = null,
    string? FrequentFlyerNumber = null,
    Guid? FrequentFlyerAirlineId = null
);

#endregion

