using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Airports.Commands.CreateAirport;

/// <summary>
/// Handler for creating a new airport.
/// </summary>
public class CreateAirportCommandHandler : ICommandHandler<CreateAirportCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateAirportCommandHandler> _logger;

    public CreateAirportCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateAirportCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateAirportCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating airport: {IataCode} - {Name}", request.IataCode, request.Name);

        var repository = _unitOfWork.Repository<Airport>();

        // Check for duplicate IATA code
        var existingByIata = await repository.FirstOrDefaultAsync(
            a => a.IataCode.ToUpper() == request.IataCode.ToUpper(), cancellationToken);
        
        if (existingByIata != null)
        {
            return Result<Guid>.Failure($"Airport with IATA code '{request.IataCode}' already exists");
        }

        // Check for duplicate ICAO code
        var existingByIcao = await repository.FirstOrDefaultAsync(
            a => a.IcaoCode.ToUpper() == request.IcaoCode.ToUpper(), cancellationToken);
        
        if (existingByIcao != null)
        {
            return Result<Guid>.Failure($"Airport with ICAO code '{request.IcaoCode}' already exists");
        }

        var airport = new Airport
        {
            Id = Guid.NewGuid(),
            IataCode = request.IataCode.ToUpper().Trim(),
            IcaoCode = request.IcaoCode.ToUpper().Trim(),
            Name = request.Name.Trim(),
            City = request.City.Trim(),
            Country = request.Country.Trim(),
            CountryCode = request.CountryCode.ToUpper().Trim(),
            Timezone = request.Timezone.Trim(),
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            IsActive = true
        };

        await repository.AddAsync(airport, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Airport created successfully: {Id}", airport.Id);
        return Result<Guid>.Success(airport.Id);
    }
}

