using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Policies.Commands.CreateBookingPolicy;

public class CreateBookingPolicyCommandHandler : ICommandHandler<CreateBookingPolicyCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateBookingPolicyCommandHandler> _logger;

    public CreateBookingPolicyCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateBookingPolicyCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateBookingPolicyCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating booking policy: {Code} - {Name}", request.Code, request.Name);

        var repository = _unitOfWork.Repository<BookingPolicy>();

        // Check for duplicate code
        var existingByCode = await repository.FirstOrDefaultAsync(
            p => p.Code.ToUpper() == request.Code.ToUpper(), cancellationToken);

        if (existingByCode != null)
        {
            return Result<Guid>.Failure($"Booking policy with code '{request.Code}' already exists");
        }

        var policy = new BookingPolicy
        {
            Id = Guid.NewGuid(),
            Code = request.Code.ToUpperInvariant().Trim(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            Type = request.Type,
            Value = request.Value,
            ErrorMessage = request.ErrorMessage.Trim(),
            AirlineId = request.AirlineId,
            DepartureAirportId = request.DepartureAirportId,
            ArrivalAirportId = request.ArrivalAirportId,
            CabinClass = request.CabinClass,
            Priority = request.Priority,
            IsActive = true
        };

        await repository.AddAsync(policy, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Booking policy created successfully: {Id}", policy.Id);
        return Result<Guid>.Success(policy.Id);
    }
}

