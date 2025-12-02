using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Promotions.Commands.UpdatePromotion;

public class UpdatePromotionCommandHandler : ICommandHandler<UpdatePromotionCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePromotionCommandHandler> _logger;

    public UpdatePromotionCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdatePromotionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(UpdatePromotionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Updating promotion: {Id}", request.Id);

        var repository = _unitOfWork.Repository<Promotion>();
        var promotion = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (promotion == null)
        {
            return Result.Failure($"Promotion with ID '{request.Id}' not found");
        }

        promotion.Name = request.Name.Trim();
        promotion.Description = request.Description?.Trim() ?? string.Empty;
        promotion.DiscountValue = request.DiscountValue;
        promotion.MaxDiscountAmount = request.MaxDiscountAmount;
        promotion.MinBookingAmount = request.MinBookingAmount;
        promotion.ValidFrom = request.ValidFrom;
        promotion.ValidTo = request.ValidTo;
        promotion.MaxTotalUses = request.MaxTotalUses;
        promotion.MaxUsesPerCustomer = request.MaxUsesPerCustomer;
        promotion.ApplicableDays = request.ApplicableDays;
        promotion.ApplicableRoutes = request.ApplicableRoutes;
        promotion.ApplicableCabinClasses = request.ApplicableCabinClasses;
        promotion.ApplicableAirlineIds = request.ApplicableAirlineIds;
        promotion.FirstTimeCustomersOnly = request.FirstTimeCustomersOnly;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Promotion updated successfully: {Id}", promotion.Id);
        return Result.Success();
    }
}

