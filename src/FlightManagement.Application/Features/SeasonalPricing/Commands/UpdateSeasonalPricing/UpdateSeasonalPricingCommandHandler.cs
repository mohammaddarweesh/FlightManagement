using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.SeasonalPricing.Commands.UpdateSeasonalPricing;

public class UpdateSeasonalPricingCommandHandler : ICommandHandler<UpdateSeasonalPricingCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public UpdateSeasonalPricingCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(UpdateSeasonalPricingCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.SeasonalPricing>();

        var seasonalPricing = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (seasonalPricing == null)
        {
            return Result.Failure($"Seasonal pricing with ID '{request.Id}' not found.");
        }

        seasonalPricing.Name = request.Name;
        seasonalPricing.Description = request.Description;
        seasonalPricing.SeasonType = request.SeasonType;
        seasonalPricing.StartDate = request.StartDate;
        seasonalPricing.EndDate = request.EndDate;
        seasonalPricing.AdjustmentPercentage = request.AdjustmentPercentage;
        seasonalPricing.AirlineId = request.AirlineId;
        seasonalPricing.DepartureAirportId = request.DepartureAirportId;
        seasonalPricing.ArrivalAirportId = request.ArrivalAirportId;
        seasonalPricing.CabinClass = request.CabinClass;
        seasonalPricing.Priority = request.Priority;
        seasonalPricing.IsActive = request.IsActive;

        repository.Update(seasonalPricing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

