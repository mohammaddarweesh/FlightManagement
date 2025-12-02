using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Seats.Commands.ReleaseSeat;

/// <summary>
/// Handler for releasing a reserved seat.
/// </summary>
public class ReleaseSeatCommandHandler : ICommandHandler<ReleaseSeatCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ReleaseSeatCommandHandler> _logger;

    public ReleaseSeatCommandHandler(IUnitOfWork unitOfWork, ILogger<ReleaseSeatCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(ReleaseSeatCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Releasing seat {FlightSeatId} for user {UserId}",
            request.FlightSeatId, request.UserId);

        var repository = _unitOfWork.Repository<FlightSeat>();
        var flightSeat = await repository.GetByIdAsync(request.FlightSeatId, cancellationToken);

        if (flightSeat == null)
        {
            return Result.Failure("Flight seat not found");
        }

        // Verify the user owns this reservation
        if (flightSeat.LockedByUserId != request.UserId)
        {
            return Result.Failure("You do not have permission to release this seat");
        }

        // Only reserved seats can be released
        if (flightSeat.Status != SeatStatus.Reserved)
        {
            return Result.Failure("Only reserved seats can be released");
        }

        // Release the seat
        flightSeat.Status = SeatStatus.Available;
        flightSeat.LockedByUserId = null;
        flightSeat.LockedUntil = null;

        repository.Update(flightSeat);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Seat released successfully: {FlightSeatId}", flightSeat.Id);
        return Result.Success();
    }
}

