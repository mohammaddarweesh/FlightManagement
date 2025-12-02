using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;
using AircraftEntity = FlightManagement.Domain.Entities.Aircraft;

namespace FlightManagement.Application.Features.Aircraft.Commands.DeleteAircraft;

/// <summary>
/// Handler for deleting (soft delete) an aircraft.
/// </summary>
public class DeleteAircraftCommandHandler : ICommandHandler<DeleteAircraftCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAircraftCommandHandler> _logger;

    public DeleteAircraftCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteAircraftCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteAircraftCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting aircraft: {AircraftId}", request.Id);

        var repository = _unitOfWork.Repository<AircraftEntity>();
        var aircraft = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (aircraft == null)
        {
            return Result.Failure($"Aircraft with ID '{request.Id}' not found");
        }

        // Soft delete
        aircraft.IsActive = false;
        repository.Update(aircraft);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Aircraft deleted (soft) successfully: {AircraftId}", aircraft.Id);
        return Result.Success();
    }
}

