using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Amenities.Commands.DeleteAmenity;

/// <summary>
/// Handler for deleting (soft delete) an amenity.
/// </summary>
public class DeleteAmenityCommandHandler : ICommandHandler<DeleteAmenityCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAmenityCommandHandler> _logger;

    public DeleteAmenityCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteAmenityCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteAmenityCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting amenity: {AmenityId}", request.Id);

        var repository = _unitOfWork.Repository<Amenity>();
        var amenity = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (amenity == null)
        {
            return Result.Failure($"Amenity with ID '{request.Id}' not found");
        }

        // Soft delete - just set IsActive to false
        amenity.IsActive = false;
        repository.Update(amenity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Amenity deleted (soft) successfully: {AmenityId}", amenity.Id);
        return Result.Success();
    }
}

