using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Airports.Commands.UpdateAirport;

/// <summary>
/// Handler for updating an existing airport.
/// </summary>
public class UpdateAirportCommandHandler : ICommandHandler<UpdateAirportCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateAirportCommandHandler> _logger;

    public UpdateAirportCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateAirportCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateAirportCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating airport: {Id}", request.Id);

        var repository = _unitOfWork.Repository<Airport>();
        var airport = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (airport == null)
        {
            return Result.Failure($"Airport with ID '{request.Id}' not found");
        }

        airport.Name = request.Name.Trim();
        airport.City = request.City.Trim();
        airport.Country = request.Country.Trim();
        airport.CountryCode = request.CountryCode.ToUpper().Trim();
        airport.Timezone = request.Timezone.Trim();
        airport.Latitude = request.Latitude;
        airport.Longitude = request.Longitude;
        airport.IsActive = request.IsActive;

        repository.Update(airport);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Airport updated successfully: {Id}", airport.Id);
        return Result.Success();
    }
}

