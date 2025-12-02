using FlightManagement.Application.Common.Messaging;
using FlightManagement.Application.Common.Models;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Application.Features.Promotions.Commands.CreatePromotion;

public class CreatePromotionCommandHandler : ICommandHandler<CreatePromotionCommand, Guid>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreatePromotionCommandHandler> _logger;

    public CreatePromotionCommandHandler(IUnitOfWork unitOfWork, ILogger<CreatePromotionCommandHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating promotion: {Code} - {Name}", request.Code, request.Name);

        var repository = _unitOfWork.Repository<Promotion>();

        // Check for duplicate code
        var existingByCode = await repository.FirstOrDefaultAsync(
            p => p.Code.ToUpper() == request.Code.ToUpper(), cancellationToken);

        if (existingByCode != null)
        {
            return Result<Guid>.Failure($"Promotion with code '{request.Code}' already exists");
        }

        var promotion = new Promotion
        {
            Id = Guid.NewGuid(),
            Code = request.Code.ToUpperInvariant().Trim(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim() ?? string.Empty,
            Type = request.Type,
            DiscountType = request.DiscountType,
            DiscountValue = request.DiscountValue,
            MaxDiscountAmount = request.MaxDiscountAmount,
            MinBookingAmount = request.MinBookingAmount,
            Currency = request.Currency.ToUpperInvariant(),
            ValidFrom = request.ValidFrom,
            ValidTo = request.ValidTo,
            MaxTotalUses = request.MaxTotalUses,
            MaxUsesPerCustomer = request.MaxUsesPerCustomer,
            ApplicableDays = request.ApplicableDays,
            ApplicableRoutes = request.ApplicableRoutes,
            ApplicableCabinClasses = request.ApplicableCabinClasses,
            ApplicableAirlineIds = request.ApplicableAirlineIds,
            FirstTimeCustomersOnly = request.FirstTimeCustomersOnly,
            Status = PromotionStatus.Draft,
            CurrentUsageCount = 0,
            IsActive = true
        };

        await repository.AddAsync(promotion, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Promotion created successfully: {Id}", promotion.Id);
        return Result<Guid>.Success(promotion.Id);
    }
}

