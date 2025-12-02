using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Bookings.Queries.GetBookingHistory;

/// <summary>
/// Handler for getting booking history.
/// </summary>
public class GetBookingHistoryQueryHandler : IQueryHandler<GetBookingHistoryQuery, List<BookingHistoryDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetBookingHistoryQueryHandler> _logger;

    public GetBookingHistoryQueryHandler(IUnitOfWork unitOfWork, ILogger<GetBookingHistoryQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<List<BookingHistoryDto>>> Handle(GetBookingHistoryQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting history for booking: {BookingId}", request.BookingId);

        // Verify booking exists
        var bookingRepo = _unitOfWork.Repository<Booking>();
        var bookingExists = await bookingRepo.Query()
            .AnyAsync(b => b.Id == request.BookingId, cancellationToken);

        if (!bookingExists)
            return Result<List<BookingHistoryDto>>.Failure($"Booking with ID '{request.BookingId}' not found");

        var historyRepo = _unitOfWork.Repository<BookingHistory>();
        var history = await historyRepo.Query()
            .Where(h => h.BookingId == request.BookingId)
            .OrderByDescending(h => h.PerformedAt)
            .ToListAsync(cancellationToken);

        var dtos = history.Select(h => new BookingHistoryDto(
            h.Id,
            h.Action,
            h.Description,
            h.OldValues,
            h.NewValues,
            h.PerformedByType,
            h.PerformedBy,
            h.PerformedAt,
            h.IpAddress
        )).ToList();

        return Result<List<BookingHistoryDto>>.Success(dtos);
    }
}

