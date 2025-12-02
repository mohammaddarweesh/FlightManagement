using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Airlines.Commands.DeleteAirline;

/// <summary>
/// Handler for deleting (soft delete) an airline.
/// </summary>
public class DeleteAirlineCommandHandler : ICommandHandler<DeleteAirlineCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAirlineCommandHandler> _logger;

    public DeleteAirlineCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteAirlineCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteAirlineCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting airline: {AirlineId}", request.Id);

        var repository = _unitOfWork.Repository<Airline>();
        var airline = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (airline == null)
        {
            return Result.Failure($"Airline with ID '{request.Id}' not found");
        }

        // Soft delete - just set IsActive to false
        airline.IsActive = false;
        repository.Update(airline);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Airline deleted (soft) successfully: {AirlineId}", airline.Id);
        return Result.Success();
    }
}

