using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Airlines.Commands.CreateAirline;

/// <summary>
/// Handler for creating a new airline.
/// </summary>
public class CreateAirlineCommandHandler : ICommandHandler<CreateAirlineCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateAirlineCommandHandler> _logger;

    public CreateAirlineCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateAirlineCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateAirlineCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating airline: {IataCode} - {Name}", request.IataCode, request.Name);

        var repository = _unitOfWork.Repository<Airline>();

        // Check for duplicate IATA code
        var existingByIata = await repository.FirstOrDefaultAsync(
            a => a.IataCode.ToUpper() == request.IataCode.ToUpper(), cancellationToken);
        
        if (existingByIata != null)
        {
            return Result<Guid>.Failure($"Airline with IATA code '{request.IataCode}' already exists");
        }

        // Check for duplicate ICAO code
        var existingByIcao = await repository.FirstOrDefaultAsync(
            a => a.IcaoCode.ToUpper() == request.IcaoCode.ToUpper(), cancellationToken);
        
        if (existingByIcao != null)
        {
            return Result<Guid>.Failure($"Airline with ICAO code '{request.IcaoCode}' already exists");
        }

        var airline = new Airline
        {
            Id = Guid.NewGuid(),
            IataCode = request.IataCode.ToUpper().Trim(),
            IcaoCode = request.IcaoCode.ToUpper().Trim(),
            Name = request.Name.Trim(),
            Country = request.Country.Trim(),
            LogoUrl = request.LogoUrl?.Trim(),
            IsActive = true
        };

        await repository.AddAsync(airline, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Airline created successfully: {Id}", airline.Id);
        return Result<Guid>.Success(airline.Id);
    }
}

