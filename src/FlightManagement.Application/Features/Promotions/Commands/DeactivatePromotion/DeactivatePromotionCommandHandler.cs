using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Promotions.Commands.DeactivatePromotion;

public class DeactivatePromotionCommandHandler : ICommandHandler<DeactivatePromotionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeactivatePromotionCommandHandler> _logger;

    public DeactivatePromotionCommandHandler(IUnitOfWork unitOfWork, ILogger<DeactivatePromotionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeactivatePromotionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deactivating promotion: {Id}", request.Id);

        var repository = _unitOfWork.Repository<Promotion>();
        var promotion = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (promotion == null)
        {
            return Result.Failure($"Promotion with ID '{request.Id}' not found");
        }

        promotion.Status = PromotionStatus.Paused;
        promotion.IsActive = false;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Promotion deactivated successfully: {Id}", promotion.Id);
        return Result.Success();
    }
}

