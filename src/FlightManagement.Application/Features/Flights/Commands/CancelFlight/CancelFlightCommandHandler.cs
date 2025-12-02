using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Flights.Commands.CancelFlight;

/// <summary>
/// Handler for cancelling a flight.
/// </summary>
public class CancelFlightCommandHandler : ICommandHandler<CancelFlightCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CancelFlightCommandHandler> _logger;

    public CancelFlightCommandHandler(IUnitOfWork unitOfWork, ILogger<CancelFlightCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(CancelFlightCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Cancelling flight: {FlightId}", request.FlightId);

        var repository = _unitOfWork.Repository<Flight>();
        var flight = await repository.GetByIdAsync(request.FlightId, cancellationToken);

        if (flight == null)
        {
            return Result.Failure($"Flight with ID '{request.FlightId}' not found");
        }

        // Check if flight can be cancelled
        if (flight.Status == FlightStatus.Cancelled)
        {
            return Result.Failure("Flight is already cancelled");
        }

        if (flight.Status == FlightStatus.Departed || 
            flight.Status == FlightStatus.InFlight || 
            flight.Status == FlightStatus.Landed || 
            flight.Status == FlightStatus.Arrived)
        {
            return Result.Failure($"Cannot cancel flight with status '{flight.Status}'");
        }

        // Cancel the flight
        flight.Status = FlightStatus.Cancelled;
        flight.IsActive = false;

        repository.Update(flight);

        // Release all reserved seats
        var flightSeatRepo = _unitOfWork.Repository<FlightSeat>();
        var reservedSeats = await flightSeatRepo.FindAsync(
            fs => fs.FlightId == request.FlightId && fs.Status == SeatStatus.Reserved,
            cancellationToken);

        foreach (var seat in reservedSeats)
        {
            seat.Status = SeatStatus.Blocked;
            seat.LockedByUserId = null;
            seat.LockedUntil = null;
            flightSeatRepo.Update(seat);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Flight cancelled successfully: {FlightId}. Reason: {Reason}", 
            flight.Id, request.CancellationReason ?? "Not specified");

        return Result.Success();
    }
}

