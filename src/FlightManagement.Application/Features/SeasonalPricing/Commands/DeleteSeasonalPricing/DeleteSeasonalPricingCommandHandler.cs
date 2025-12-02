using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Repositories;

namespace FlightManagement.Application.Features.SeasonalPricing.Commands.DeleteSeasonalPricing;

public class DeleteSeasonalPricingCommandHandler : ICommandHandler<DeleteSeasonalPricingCommand>
{
    private readonly IUnitOfWork _unitOfWork;

    public DeleteSeasonalPricingCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteSeasonalPricingCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Domain.Entities.SeasonalPricing>();

        var seasonalPricing = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (seasonalPricing == null)
        {
            return Result.Failure($"Seasonal pricing with ID '{request.Id}' not found.");
        }

        repository.Delete(seasonalPricing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

