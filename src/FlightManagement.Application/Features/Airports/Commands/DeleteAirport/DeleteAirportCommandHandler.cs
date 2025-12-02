using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Airports.Commands.DeleteAirport;

/// <summary>
/// Handler for deleting (soft delete) an airport.
/// </summary>
public class DeleteAirportCommandHandler : ICommandHandler<DeleteAirportCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAirportCommandHandler> _logger;

    public DeleteAirportCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteAirportCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteAirportCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting airport: {AirportId}", request.Id);

        var repository = _unitOfWork.Repository<Airport>();
        var airport = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (airport == null)
        {
            return Result.Failure($"Airport with ID '{request.Id}' not found");
        }

        // Soft delete - just set IsActive to false
        airport.IsActive = false;
        repository.Update(airport);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Airport deleted (soft) successfully: {AirportId}", airport.Id);
        return Result.Success();
    }
}

