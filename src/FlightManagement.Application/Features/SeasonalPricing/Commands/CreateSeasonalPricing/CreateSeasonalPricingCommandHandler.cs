using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.SeasonalPricing.Commands.CreateSeasonalPricing;

public class CreateSeasonalPricingCommandHandler : ICommandHandler<CreateSeasonalPricingCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;

    public CreateSeasonalPricingCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Guid>> Handle(CreateSeasonalPricingCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.SeasonalPricing>();

        var seasonalPricing = new Domain.Entities.SeasonalPricing
        {
            Name = request.Name,
            Description = request.Description,
            SeasonType = request.SeasonType,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            AdjustmentPercentage = request.AdjustmentPercentage,
            AirlineId = request.AirlineId,
            DepartureAirportId = request.DepartureAirportId,
            ArrivalAirportId = request.ArrivalAirportId,
            CabinClass = request.CabinClass,
            Priority = request.Priority,
            IsActive = request.IsActive
        };

        await repository.AddAsync(seasonalPricing, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(seasonalPricing.Id);
    }
}

