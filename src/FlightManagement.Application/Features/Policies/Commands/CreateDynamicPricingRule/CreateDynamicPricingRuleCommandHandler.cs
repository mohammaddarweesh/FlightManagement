using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Policies.Commands.CreateDynamicPricingRule;

public class CreateDynamicPricingRuleCommandHandler : ICommandHandler<CreateDynamicPricingRuleCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateDynamicPricingRuleCommandHandler> _logger;

    public CreateDynamicPricingRuleCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateDynamicPricingRuleCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreateDynamicPricingRuleCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating dynamic pricing rule: {Name} ({RuleType})", request.Name, request.RuleType);

        var rule = new DynamicPricingRule
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim(),
            RuleType = request.RuleType,
            AdjustmentPercentage = request.AdjustmentPercentage,
            FixedAdjustment = request.FixedAdjustment,
            Currency = request.Currency.ToUpperInvariant(),
            Priority = request.Priority,
            ApplicableDays = request.ApplicableDays,
            SeasonType = request.SeasonType,
            SeasonStartDate = request.SeasonStartDate,
            SeasonEndDate = request.SeasonEndDate,
            MinBookingPercentage = request.MinBookingPercentage,
            MaxBookingPercentage = request.MaxBookingPercentage,
            MinDaysBeforeDeparture = request.MinDaysBeforeDeparture,
            MaxDaysBeforeDeparture = request.MaxDaysBeforeDeparture,
            StartHour = request.StartHour,
            EndHour = request.EndHour,
            AirlineId = request.AirlineId,
            DepartureAirportId = request.DepartureAirportId,
            ArrivalAirportId = request.ArrivalAirportId,
            CabinClass = request.CabinClass,
            IsActive = true
        };

        await _unitOfWork.Repository<DynamicPricingRule>().AddAsync(rule, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Dynamic pricing rule created successfully: {Id}", rule.Id);
        return Result<Guid>.Success(rule.Id);
    }
}

