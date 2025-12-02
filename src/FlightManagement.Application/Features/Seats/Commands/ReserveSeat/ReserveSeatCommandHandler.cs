using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Seats.Commands.ReserveSeat;

/// <summary>
/// Handler for temporarily reserving a seat.
/// </summary>
public class ReserveSeatCommandHandler : ICommandHandler<ReserveSeatCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReserveSeatCommandHandler> _logger;

    public ReserveSeatCommandHandler(IUnitOfWork unitOfWork, ILogger<ReserveSeatCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(ReserveSeatCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Reserving seat {SeatId} on flight {FlightId} for user {UserId}",
            request.SeatId, request.FlightId, request.UserId);

        var repository = _unitOfWork.Repository<FlightSeat>();

        // Find the flight seat
        var flightSeat = await repository.FirstOrDefaultAsync(
            fs => fs.FlightId == request.FlightId && fs.SeatId == request.SeatId,
            cancellationToken,
            fs => fs.Seat
        );

        if (flightSeat == null)
        {
            return Result<Guid>.Failure("Seat not found for this flight");
        }

        // Check if seat can be reserved
        if (!flightSeat.CanBeReserved)
        {
            return Result<Guid>.Failure($"Seat {flightSeat.Seat.SeatNumber} is not available for reservation");
        }

        // Reserve the seat
        flightSeat.Status = SeatStatus.Reserved;
        flightSeat.LockedByUserId = request.UserId;
        flightSeat.LockedUntil = DateTime.UtcNow.AddMinutes(request.LockDurationMinutes);

        repository.Update(flightSeat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seat {SeatNumber} reserved successfully until {LockedUntil}",
            flightSeat.Seat.SeatNumber, flightSeat.LockedUntil);

        return Result<Guid>.Success(flightSeat.Id);
    }
}

