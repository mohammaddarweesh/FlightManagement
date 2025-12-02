using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;
using AircraftEntity = FlightManagement.Domain.Entities.Aircraft;

namespace FlightManagement.Application.Features.Aircraft.Commands.UpdateAircraft;

/// <summary>
/// Handler for updating an aircraft.
/// </summary>
public class UpdateAircraftCommandHandler : ICommandHandler<UpdateAircraftCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateAircraftCommandHandler> _logger;

    public UpdateAircraftCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateAircraftCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateAircraftCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating aircraft: {AircraftId}", request.Id);

        var repository = _unitOfWork.Repository<AircraftEntity>();
        var aircraft = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (aircraft == null)
        {
            return Result.Failure($"Aircraft with ID '{request.Id}' not found");
        }

        // Check for duplicate registration
        var existingByReg = await repository.FirstOrDefaultAsync(
            a => a.RegistrationNumber == request.RegistrationNumber.ToUpper() && a.Id != request.Id,
            cancellationToken);
        if (existingByReg != null)
        {
            return Result.Failure($"Aircraft with registration '{request.RegistrationNumber}' already exists");
        }

        aircraft.Model = request.Model;
        aircraft.Manufacturer = request.Manufacturer;
        aircraft.RegistrationNumber = request.RegistrationNumber.ToUpper();
        aircraft.IsActive = request.IsActive;

        repository.Update(aircraft);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Aircraft updated successfully: {AircraftId}", aircraft.Id);
        return Result.Success();
    }
}

