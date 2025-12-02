using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Airlines.Commands.UpdateAirline;

/// <summary>
/// Handler for updating an airline.
/// </summary>
public class UpdateAirlineCommandHandler : ICommandHandler<UpdateAirlineCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdateAirlineCommandHandler> _logger;

    public UpdateAirlineCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateAirlineCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdateAirlineCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating airline: {AirlineId}", request.Id);

        var repository = _unitOfWork.Repository<Airline>();
        var airline = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (airline == null)
        {
            return Result.Failure($"Airline with ID '{request.Id}' not found");
        }

        // Check for duplicate IATA code
        var existingByIata = await repository.FirstOrDefaultAsync(
            a => a.IataCode == request.IataCode.ToUpper() && a.Id != request.Id,
            cancellationToken);
        if (existingByIata != null)
        {
            return Result.Failure($"Airline with IATA code '{request.IataCode}' already exists");
        }

        airline.IataCode = request.IataCode.ToUpper();
        airline.IcaoCode = request.IcaoCode.ToUpper();
        airline.Name = request.Name;
        airline.Country = request.Country;
        airline.LogoUrl = request.LogoUrl;
        airline.IsActive = request.IsActive;

        repository.Update(airline);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Airline updated successfully: {AirlineId}", airline.Id);
        return Result.Success();
    }
}

