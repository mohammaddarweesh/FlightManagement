using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Amenities.Commands.UpdateAmenity;

/// <summary>
/// Handler for updating an amenity.
/// </summary>
public class UpdateAmenityCommandHandler : ICommandHandler<UpdateAmenityCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateAmenityCommandHandler> _logger;

    public UpdateAmenityCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateAmenityCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateAmenityCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating amenity: {AmenityId}", request.Id);

        var repository = _unitOfWork.Repository<Amenity>();
        var amenity = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (amenity == null)
        {
            return Result.Failure($"Amenity with ID '{request.Id}' not found");
        }

        // Check for duplicate code
        var existingByCode = await repository.FirstOrDefaultAsync(
            a => a.Code == request.Code.ToUpper() && a.Id != request.Id,
            cancellationToken);
        if (existingByCode != null)
        {
            return Result.Failure($"Amenity with code '{request.Code}' already exists");
        }

        amenity.Code = request.Code.ToUpper();
        amenity.Name = request.Name;
        amenity.Description = request.Description;
        amenity.Category = request.Category;
        amenity.IconUrl = request.IconUrl;
        amenity.IsActive = request.IsActive;

        repository.Update(amenity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Amenity updated successfully: {AmenityId}", amenity.Id);
        return Result.Success();
    }
}

