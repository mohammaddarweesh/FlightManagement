using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Policies.Commands.CreateBlackoutDate;

public class CreateBlackoutDateCommandHandler : ICommandHandler<CreateBlackoutDateCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateBlackoutDateCommandHandler> _logger;

    public CreateBlackoutDateCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateBlackoutDateCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateBlackoutDateCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating blackout date: {Name} ({StartDate} - {EndDate})",
            request.Name, request.StartDate, request.EndDate);

        var blackoutDate = new BlackoutDate
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            BlocksBookings = request.BlocksBookings,
            BlocksPromotions = request.BlocksPromotions,
            AirlineId = request.AirlineId,
            DepartureAirportId = request.DepartureAirportId,
            ArrivalAirportId = request.ArrivalAirportId,
            IsActive = true
        };

        await _unitOfWork.Repository<BlackoutDate>().AddAsync(blackoutDate, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Blackout date created successfully: {Id}", blackoutDate.Id);
        return Result<Guid>.Success(blackoutDate.Id);
    }
}

