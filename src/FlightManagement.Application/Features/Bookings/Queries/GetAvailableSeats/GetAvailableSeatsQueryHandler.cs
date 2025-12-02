using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Bookings.Queries.GetAvailableSeats;

/// <summary>
/// Handler for getting available seats.
/// </summary>
public class GetAvailableSeatsQueryHandler : IQueryHandler<GetAvailableSeatsQuery, FlightSeatMapDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAvailableSeatsQueryHandler> _logger;

    public GetAvailableSeatsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAvailableSeatsQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<FlightSeatMapDto>> Handle(GetAvailableSeatsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Getting available seats for flight: {FlightId}", request.FlightId);

        var flightRepo = _unitOfWork.Repository<Flight>();
        var flight = await flightRepo.GetByIdAsync(request.FlightId, cancellationToken);

        if (flight == null)
            return Result<FlightSeatMapDto>.Failure($"Flight with ID '{request.FlightId}' not found");

        var flightSeatRepo = _unitOfWork.Repository<FlightSeat>();
        var query = flightSeatRepo.Query()
            .Include(fs => fs.Seat)
            .Where(fs => fs.FlightId == request.FlightId);

        if (request.CabinClass.HasValue)
            query = query.Where(fs => fs.Seat != null && fs.Seat.CabinClass == request.CabinClass.Value);

        var flightSeats = await query
            .OrderBy(fs => fs.Seat!.Row)
            .ThenBy(fs => fs.Seat!.Column)
            .ToListAsync(cancellationToken);

        // Group by row
        var rows = flightSeats
            .GroupBy(fs => fs.Seat!.Row)
            .Select(g => new SeatRowDto(
                g.Key,
                g.First().Seat!.CabinClass,
                g.Select(fs => new SeatDto(
                    fs.Id,
                    fs.Seat!.SeatNumber,
                    fs.Seat.SeatType,
                    fs.Status,
                    fs.Status == SeatStatus.Available,
                    fs.Price,
                    fs.Seat.SeatType == SeatType.Window,
                    fs.Seat.SeatType == SeatType.Aisle,
                    fs.Seat.IsEmergencyExit,
                    fs.Seat.HasExtraLegroom
                )).ToList()
            ))
            .ToList();

        return Result<FlightSeatMapDto>.Success(new FlightSeatMapDto(
            flight.Id,
            flight.FlightNumber,
            rows
        ));
    }
}

