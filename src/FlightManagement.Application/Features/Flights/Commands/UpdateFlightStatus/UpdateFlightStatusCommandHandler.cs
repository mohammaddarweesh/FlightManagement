using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Flights.Commands.UpdateFlightStatus;

/// <summary>
/// Handler for updating a flight's status.
/// </summary>
public class UpdateFlightStatusCommandHandler : ICommandHandler<UpdateFlightStatusCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateFlightStatusCommandHandler> _logger;

    public UpdateFlightStatusCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateFlightStatusCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateFlightStatusCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating flight status: {FlightId} to {Status}", request.FlightId, request.NewStatus);

        var repository = _unitOfWork.Repository<Flight>();
        var flight = await repository.GetByIdAsync(request.FlightId, cancellationToken);

        if (flight == null)
        {
            return Result.Failure($"Flight with ID '{request.FlightId}' not found");
        }

        // Validate status transition
        if (!IsValidStatusTransition(flight.Status, request.NewStatus))
        {
            return Result.Failure($"Invalid status transition from '{flight.Status}' to '{request.NewStatus}'");
        }

        flight.Status = request.NewStatus;

        if (request.ActualDepartureTime.HasValue)
            flight.ActualDepartureTime = request.ActualDepartureTime;

        if (request.ActualArrivalTime.HasValue)
            flight.ActualArrivalTime = request.ActualArrivalTime;

        repository.Update(flight);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Flight status updated successfully: {FlightId}", flight.Id);
        return Result.Success();
    }

    private static bool IsValidStatusTransition(FlightStatus current, FlightStatus next)
    {
        // Define valid status transitions
        return (current, next) switch
        {
            (FlightStatus.Scheduled, FlightStatus.Boarding) => true,
            (FlightStatus.Scheduled, FlightStatus.Delayed) => true,
            (FlightStatus.Scheduled, FlightStatus.Cancelled) => true,
            (FlightStatus.Delayed, FlightStatus.Boarding) => true,
            (FlightStatus.Delayed, FlightStatus.Cancelled) => true,
            (FlightStatus.Boarding, FlightStatus.Departed) => true,
            (FlightStatus.Departed, FlightStatus.InFlight) => true,
            (FlightStatus.InFlight, FlightStatus.Landed) => true,
            (FlightStatus.Landed, FlightStatus.Arrived) => true,
            _ => false
        };
    }
}

