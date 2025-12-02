using FlightManagement.API.Authorization;
using FlightManagement.Application.Features.Bookings.Commands.CancelBooking;
using FlightManagement.Application.Features.Bookings.Queries.GetBookingById;
using FlightManagement.Application.Features.Bookings.Queries.GetBookingByReference;
using FlightManagement.Application.Features.Bookings.Queries.GetBookingHistory;
using FlightManagement.Application.Features.Bookings.Queries.GetCustomerBookings;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.API.Controllers.Admin;

/// <summary>
/// Admin controller for managing bookings.
/// </summary>
[Route("api/admin/bookings")]
[RequireAdmin]
public class BookingsController : BaseApiController
{
    private readonly IUnitOfWork _unitOfWork;

    public BookingsController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Get all bookings with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] BookingStatus? status = null,
        [FromQuery] PaymentStatus? paymentStatus = null,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] string? searchTerm = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var bookingRepo = _unitOfWork.Repository<Booking>();
        var query = bookingRepo.Query()
            .Include(b => b.Customer)
            .Include(b => b.Segments)
                .ThenInclude(s => s.Flight)
            .Include(b => b.Passengers)
            .AsQueryable();

        if (status.HasValue)
            query = query.Where(b => b.Status == status.Value);

        if (paymentStatus.HasValue)
            query = query.Where(b => b.PaymentStatus == paymentStatus.Value);

        if (fromDate.HasValue)
            query = query.Where(b => b.BookingDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(b => b.BookingDate <= toDate.Value);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var term = searchTerm.ToUpper();
            query = query.Where(b => 
                b.BookingReference.Contains(term) ||
                (b.Customer != null && (b.Customer.FirstName + " " + b.Customer.LastName).ToUpper().Contains(term)) ||
                b.ContactEmail.ToUpper().Contains(term));
        }

        var totalCount = await query.CountAsync();
        var bookings = await query
            .OrderByDescending(b => b.BookingDate)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = bookings.Select(b => new
        {
            b.Id,
            b.BookingReference,
            CustomerName = b.Customer != null ? $"{b.Customer.FirstName} {b.Customer.LastName}" : "Unknown",
            b.Status,
            b.TripType,
            b.TotalAmount,
            b.Currency,
            b.PaymentStatus,
            PassengerCount = b.Passengers.Count,
            b.BookingDate,
            b.CreatedAt
        }).ToList();

        return Ok(new
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
        });
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
    /// Get bookings for a specific customer.
    /// </summary>
    [HttpGet("customer/{customerId:guid}")]
    public async Task<IActionResult> GetCustomerBookings(
        Guid customerId,
        [FromQuery] BookingStatus? status = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var query = new GetCustomerBookingsQuery(customerId, status, null, null, page, pageSize);
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
    /// Cancel a booking (admin).
    /// </summary>
    [HttpPost("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel(Guid id, [FromBody] AdminCancelBookingRequest? request = null)
    {
        var command = new CancelBookingCommand(id, request?.CancellationReason);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }
}

public record AdminCancelBookingRequest(string? CancellationReason = null);

