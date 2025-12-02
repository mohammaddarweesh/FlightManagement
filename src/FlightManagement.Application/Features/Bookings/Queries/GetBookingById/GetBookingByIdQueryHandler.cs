using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Application.Features.Bookings.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Bookings.Queries.GetBookingById;

/// <summary>
/// Handler for getting a booking by ID.
/// </summary>
public class GetBookingByIdQueryHandler : IQueryHandler<GetBookingByIdQuery, BookingDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetBookingByIdQueryHandler> _logger;

    public GetBookingByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetBookingByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<BookingDto>> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting booking by ID: {BookingId}", request.BookingId);

        var bookingRepo = _unitOfWork.Repository<Booking>();
        var booking = await bookingRepo.Query()
            .Include(b => b.Customer)
            .Include(b => b.CancellationPolicy)
            .Include(b => b.Passengers)
                .ThenInclude(p => p.PassengerSeats)
                    .ThenInclude(ps => ps.BookingSegment)
                        .ThenInclude(bs => bs!.Flight)
            .Include(b => b.Segments)
                .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f!.DepartureAirport)
            .Include(b => b.Segments)
                .ThenInclude(s => s.Flight)
                    .ThenInclude(f => f!.ArrivalAirport)
            .Include(b => b.Extras)
                .ThenInclude(e => e.Passenger)
            .FirstOrDefaultAsync(b => b.Id == request.BookingId, cancellationToken);

        if (booking == null)
            return Result<BookingDto>.Failure($"Booking with ID '{request.BookingId}' not found");

        var dto = MapToDto(booking);
        return Result<BookingDto>.Success(dto);
    }

    private static BookingDto MapToDto(Booking booking)
    {
        return new BookingDto(
            booking.Id,
            booking.BookingReference,
            booking.CustomerId,
            $"{booking.Customer?.FirstName} {booking.Customer?.LastName}",
            booking.Status,
            booking.TripType,
            booking.BookingDate,
            booking.ExpiresAt,
            booking.BaseFare,
            booking.TaxAmount,
            booking.ServiceFee,
            booking.SeatSelectionFees,
            booking.ExtrasFees,
            booking.DiscountAmount,
            booking.TotalAmount,
            booking.Currency,
            booking.ContactEmail,
            booking.ContactPhone,
            booking.SpecialRequests,
            booking.PaymentStatus,
            booking.PaidAmount,
            booking.PaymentDueDate,
            booking.CancellationPolicyId,
            booking.CancellationPolicy?.Name,
            booking.CancelledAt,
            booking.CancellationReason,
            booking.RefundAmount,
            booking.RefundStatus,
            booking.Passengers.Select(MapPassengerToDto).ToList(),
            booking.Segments.Select(MapSegmentToDto).ToList(),
            booking.Extras.Select(MapExtraToDto).ToList(),
            booking.CreatedAt
        );
    }

    private static PassengerDto MapPassengerToDto(Passenger p)
    {
        return new PassengerDto(
            p.Id,
            p.PassengerType,
            p.Title,
            p.FirstName,
            p.MiddleName,
            p.LastName,
            p.DateOfBirth,
            p.Gender,
            p.Nationality,
            p.PassportNumber,
            p.PassportIssuingCountry,
            p.PassportExpiryDate,
            p.Email,
            p.Phone,
            p.MealPreference,
            p.SpecialAssistance,
            p.IsPrimaryContact,
            p.IsLeadPassenger,
            p.PassengerSeats.Select(ps => new PassengerSeatDto(
                ps.Id,
                ps.BookingSegmentId,
                ps.BookingSegment?.Flight?.FlightNumber ?? "",
                ps.SeatNumber,
                ps.SeatFee,
                ps.AssignmentType
            )).ToList()
        );
    }

    private static BookingSegmentDto MapSegmentToDto(BookingSegment s)
    {
        return new BookingSegmentDto(
            s.Id,
            s.FlightId,
            s.Flight?.FlightNumber ?? "",
            s.SegmentOrder,
            s.CabinClass,
            s.Flight?.DepartureAirport?.IataCode ?? "",
            s.Flight?.DepartureAirport?.Name ?? "",
            s.Flight?.ArrivalAirport?.IataCode ?? "",
            s.Flight?.ArrivalAirport?.Name ?? "",
            s.Flight?.ScheduledDepartureTime ?? DateTime.MinValue,
            s.Flight?.ScheduledArrivalTime ?? DateTime.MinValue,
            s.Flight?.DepartureTerminal,
            s.Flight?.DepartureGate,
            s.BaseFarePerPax,
            s.TaxPerPax,
            s.SegmentSubtotal,
            s.Status,
            s.CheckInOpenAt,
            s.CheckedBaggageAllowanceKg,
            s.CabinBaggageAllowanceKg
        );
    }

    private static BookingExtraDto MapExtraToDto(BookingExtra e)
    {
        return new BookingExtraDto(
            e.Id,
            e.ExtraType,
            e.Description,
            e.Quantity,
            e.UnitPrice,
            e.TotalPrice,
            e.Currency,
            e.Status,
            e.BookingSegmentId,
            e.PassengerId,
            e.Passenger != null ? $"{e.Passenger.FirstName} {e.Passenger.LastName}" : null
        );
    }
}

