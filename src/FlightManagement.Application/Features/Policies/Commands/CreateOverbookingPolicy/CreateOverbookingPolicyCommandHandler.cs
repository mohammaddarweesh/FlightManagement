using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Policies.Commands.CreateOverbookingPolicy;

public class CreateOverbookingPolicyCommandHandler : ICommandHandler<CreateOverbookingPolicyCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateOverbookingPolicyCommandHandler> _logger;

    public CreateOverbookingPolicyCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateOverbookingPolicyCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateOverbookingPolicyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating overbooking policy: {Name} for airline {AirlineId}",
            request.Name, request.AirlineId);

        // Verify airline exists
        var airline = await _unitOfWork.Repository<Airline>().GetByIdAsync(request.AirlineId, cancellationToken);
        if (airline == null)
        {
            return Result<Guid>.Failure($"Airline with ID '{request.AirlineId}' not found");
        }

        var policy = new OverbookingPolicy
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            AirlineId = request.AirlineId,
            MaxOverbookingPercentage = request.MaxOverbookingPercentage,
            MaxOverbookedSeats = request.MaxOverbookedSeats,
            DepartureAirportId = request.DepartureAirportId,
            ArrivalAirportId = request.ArrivalAirportId,
            CabinClass = request.CabinClass,
            Priority = request.Priority,
            IsActive = true
        };

        await _unitOfWork.Repository<OverbookingPolicy>().AddAsync(policy, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Overbooking policy created successfully: {Id}", policy.Id);
        return Result<Guid>.Success(policy.Id);
    }
}

