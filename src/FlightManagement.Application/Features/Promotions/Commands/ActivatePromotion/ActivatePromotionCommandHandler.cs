using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Promotions.Commands.ActivatePromotion;

public class ActivatePromotionCommandHandler : ICommandHandler<ActivatePromotionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ActivatePromotionCommandHandler> _logger;

    public ActivatePromotionCommandHandler(IUnitOfWork unitOfWork, ILogger<ActivatePromotionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(ActivatePromotionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Activating promotion: {Id}", request.Id);

        var repository = _unitOfWork.Repository<Promotion>();
        var promotion = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (promotion == null)
        {
            return Result.Failure($"Promotion with ID '{request.Id}' not found");
        }

        if (promotion.Status == PromotionStatus.Expired)
        {
            return Result.Failure("Cannot activate an expired promotion");
        }

        if (promotion.Status == PromotionStatus.Exhausted)
        {
            return Result.Failure("Cannot activate an exhausted promotion");
        }

        promotion.Status = PromotionStatus.Active;
        promotion.IsActive = true;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Promotion activated successfully: {Id}", promotion.Id);
        return Result.Success();
    }
}

