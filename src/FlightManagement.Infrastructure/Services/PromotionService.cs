using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using FlightManagement.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace FlightManagement.Infrastructure.Services;

public class PromotionService : IPromotionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<PromotionService> _logger;

    public PromotionService(IUnitOfWork unitOfWork, ILogger<PromotionService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PromotionValidationResult> ValidatePromotionAsync(
        string promoCode,
        Guid customerId,
        decimal bookingAmount,
        List<PromotionFlightInfo> flights,
        CancellationToken cancellationToken = default)
    {
        var promoRepo = _unitOfWork.Repository<Promotion>();
        var promotion = await promoRepo.Query()
            .FirstOrDefaultAsync(p => p.Code == promoCode.ToUpperInvariant() && p.IsActive, cancellationToken);

        if (promotion == null)
            return new PromotionValidationResult(false, null, null, "Invalid promotion code", null);

        // Check status
        if (promotion.Status != PromotionStatus.Active)
            return new PromotionValidationResult(false, null, null, "Promotion is not active", null);

        // Check date validity
        var now = DateTime.UtcNow;
        if (now < promotion.ValidFrom)
            return new PromotionValidationResult(false, null, null, "Promotion is not yet valid", null);
        if (now > promotion.ValidTo)
            return new PromotionValidationResult(false, null, null, "Promotion has expired", null);

        // Check total usage limit
        if (promotion.MaxTotalUses.HasValue && promotion.CurrentUsageCount >= promotion.MaxTotalUses.Value)
            return new PromotionValidationResult(false, null, null, "Promotion usage limit has been reached", null);

        // Check per-customer usage limit
        if (promotion.MaxUsesPerCustomer.HasValue)
        {
            var customerUsageCount = await GetCustomerUsageCountAsync(promotion.Id, customerId, cancellationToken);
            if (customerUsageCount >= promotion.MaxUsesPerCustomer.Value)
                return new PromotionValidationResult(false, null, null, 
                    $"You have already used this promotion {customerUsageCount} time(s). Maximum allowed: {promotion.MaxUsesPerCustomer.Value}", null);
        }

        // Check minimum booking amount
        if (promotion.MinBookingAmount.HasValue && bookingAmount < promotion.MinBookingAmount.Value)
            return new PromotionValidationResult(false, null, null, 
                $"Minimum booking amount of {promotion.MinBookingAmount.Value} {promotion.Currency} required", null);

        // Check first-time customer restriction
        if (promotion.FirstTimeCustomersOnly)
        {
            var hasBookings = await _unitOfWork.Repository<Booking>()
                .Query()
                .AnyAsync(b => b.CustomerId == customerId && b.Status != BookingStatus.Cancelled, cancellationToken);
            if (hasBookings)
                return new PromotionValidationResult(false, null, null, "This promotion is only for first-time customers", null);
        }

        // Check day of week
        var travelDate = flights.Min(f => f.DepartureDate);
        var dayOfWeek = (DayOfWeekFlag)(1 << (int)travelDate.DayOfWeek);
        if (!promotion.ApplicableDays.HasFlag(dayOfWeek))
            return new PromotionValidationResult(false, null, null, "Promotion is not valid for the selected travel day", null);

        // Check applicable routes
        if (!string.IsNullOrEmpty(promotion.ApplicableRoutes))
        {
            if (!IsRouteApplicable(promotion.ApplicableRoutes, flights))
                return new PromotionValidationResult(false, null, null, "Promotion is not valid for the selected route", null);
        }

        // Check applicable airlines
        if (!string.IsNullOrEmpty(promotion.ApplicableAirlineIds))
        {
            var airlineIds = JsonSerializer.Deserialize<List<Guid>>(promotion.ApplicableAirlineIds) ?? new List<Guid>();
            if (!flights.All(f => airlineIds.Contains(f.AirlineId)))
                return new PromotionValidationResult(false, null, null, "Promotion is not valid for the selected airline", null);
        }

        // Calculate estimated discount
        var discountResult = await CalculateDiscountAsync(promotion.Id, bookingAmount, flights.Count, cancellationToken);

        return new PromotionValidationResult(true, promotion.Id, promotion.Name, null, discountResult.DiscountAmount);
    }

    public async Task<PromotionDiscountResult> CalculateDiscountAsync(
        Guid promotionId,
        decimal bookingAmount,
        int passengerCount,
        CancellationToken cancellationToken = default)
    {
        var promotion = await _unitOfWork.Repository<Promotion>()
            .GetByIdAsync(promotionId, cancellationToken);

        if (promotion == null)
            return new PromotionDiscountResult(0, DiscountType.Percentage, 0, null, "USD");

        decimal discountAmount = promotion.DiscountType switch
        {
            DiscountType.Percentage => bookingAmount * (promotion.DiscountValue / 100),
            DiscountType.FixedAmount => promotion.DiscountValue,
            DiscountType.PerPassenger => promotion.DiscountValue * passengerCount,
            _ => 0
        };

        // Apply max discount cap if set
        if (promotion.MaxDiscountAmount.HasValue)
            discountAmount = Math.Min(discountAmount, promotion.MaxDiscountAmount.Value);

        return new PromotionDiscountResult(
            discountAmount,
            promotion.DiscountType,
            promotion.DiscountValue,
            promotion.MaxDiscountAmount,
            promotion.Currency
        );
    }

    public async Task RecordPromotionUsageAsync(
        Guid promotionId,
        Guid customerId,
        Guid bookingId,
        decimal discountAmount,
        CancellationToken cancellationToken = default)
    {
        var usageRepo = _unitOfWork.Repository<PromotionUsage>();
        var promoRepo = _unitOfWork.Repository<Promotion>();

        var usage = new PromotionUsage
        {
            Id = Guid.NewGuid(),
            PromotionId = promotionId,
            CustomerId = customerId,
            BookingId = bookingId,
            DiscountAmount = discountAmount
        };

        await usageRepo.AddAsync(usage, cancellationToken);

        // Update promotion usage count
        var promotion = await promoRepo.GetByIdAsync(promotionId, cancellationToken);
        if (promotion != null)
        {
            promotion.CurrentUsageCount++;

            // Check if promotion is exhausted
            if (promotion.MaxTotalUses.HasValue && promotion.CurrentUsageCount >= promotion.MaxTotalUses.Value)
            {
                promotion.Status = PromotionStatus.Exhausted;
            }
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> GetCustomerUsageCountAsync(
        Guid promotionId,
        Guid customerId,
        CancellationToken cancellationToken = default)
    {
        return await _unitOfWork.Repository<PromotionUsage>()
            .Query()
            .CountAsync(u => u.PromotionId == promotionId && u.CustomerId == customerId, cancellationToken);
    }

    private bool IsRouteApplicable(string applicableRoutes, List<PromotionFlightInfo> flights)
    {
        try
        {
            var routes = JsonSerializer.Deserialize<List<RouteInfo>>(applicableRoutes);
            if (routes == null || routes.Count == 0) return true;

            return flights.All(f => routes.Any(r =>
                (r.DepartureAirportId == null || r.DepartureAirportId == f.DepartureAirportId) &&
                (r.ArrivalAirportId == null || r.ArrivalAirportId == f.ArrivalAirportId)));
        }
        catch
        {
            return true; // If parsing fails, allow the promotion
        }
    }

    private record RouteInfo(Guid? DepartureAirportId, Guid? ArrivalAirportId);
}

