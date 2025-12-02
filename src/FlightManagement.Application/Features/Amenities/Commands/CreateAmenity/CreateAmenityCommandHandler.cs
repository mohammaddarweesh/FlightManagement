using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Amenities.Commands.CreateAmenity;

/// <summary>
/// Handler for creating a new amenity.
/// </summary>
public class CreateAmenityCommandHandler : ICommandHandler<CreateAmenityCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateAmenityCommandHandler> _logger;

    public CreateAmenityCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateAmenityCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateAmenityCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating amenity: {Code} - {Name}", request.Code, request.Name);

        var repository = _unitOfWork.Repository<Amenity>();

        // Check for duplicate code
        var existingByCode = await repository.FirstOrDefaultAsync(
            a => a.Code == request.Code.ToUpper(),
            cancellationToken);
        if (existingByCode != null)
        {
            return Result<Guid>.Failure($"Amenity with code '{request.Code}' already exists");
        }

        var amenity = new Amenity
        {
            Code = request.Code.ToUpper(),
            Name = request.Name,
            Description = request.Description,
            Category = request.Category,
            IconUrl = request.IconUrl,
            IsActive = true
        };

        await repository.AddAsync(amenity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Amenity created successfully: {AmenityId}", amenity.Id);
        return Result<Guid>.Success(amenity.Id);
    }
}

