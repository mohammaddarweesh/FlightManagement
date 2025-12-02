using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds promotional codes.
/// </summary>
public class PromotionSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PromotionSeeder> _logger;

    public PromotionSeeder(ApplicationDbContext context, ILogger<PromotionSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Set<Promotion>().AnyAsync())
        {
            _logger.LogInformation("Promotions already seeded");
            return;
        }

        var now = DateTime.UtcNow;

        var promotions = new List<Promotion>
        {
            // Welcome discount for new customers
            new()
            {
                Id = Guid.NewGuid(),
                Code = "WELCOME10",
                Name = "Welcome Discount",
                Description = "10% off your first booking",
                Type = PromotionType.FirstBooking,
                DiscountType = DiscountType.Percentage,
                DiscountValue = 10m,
                MaxDiscountAmount = 100m,
                Currency = "USD",
                Status = PromotionStatus.Active,
                ValidFrom = now.AddDays(-30),
                ValidTo = now.AddMonths(12),
                MaxUsesPerCustomer = 1,
                FirstTimeCustomersOnly = true,
                ApplicableDays = DayOfWeekFlag.AllDays,
                IsActive = true
            },
            // Summer sale
            new()
            {
                Id = Guid.NewGuid(),
                Code = "SUMMER25",
                Name = "Summer Sale",
                Description = "25% off summer travel",
                Type = PromotionType.General,
                DiscountType = DiscountType.Percentage,
                DiscountValue = 25m,
                MaxDiscountAmount = 200m,
                MinBookingAmount = 300m,
                Currency = "USD",
                Status = PromotionStatus.Active,
                ValidFrom = now,
                ValidTo = now.AddMonths(3),
                MaxTotalUses = 1000,
                ApplicableDays = DayOfWeekFlag.AllDays,
                IsActive = true
            },
            // Flat $50 off
            new()
            {
                Id = Guid.NewGuid(),
                Code = "SAVE50",
                Name = "$50 Off",
                Description = "Save $50 on bookings over $400",
                Type = PromotionType.General,
                DiscountType = DiscountType.FixedAmount,
                DiscountValue = 50m,
                MinBookingAmount = 400m,
                Currency = "USD",
                Status = PromotionStatus.Active,
                ValidFrom = now,
                ValidTo = now.AddMonths(6),
                ApplicableDays = DayOfWeekFlag.AllDays,
                IsActive = true
            },
            // Flash sale
            new()
            {
                Id = Guid.NewGuid(),
                Code = "FLASH30",
                Name = "Flash Sale 30% Off",
                Description = "Limited time 30% discount",
                Type = PromotionType.FlashSale,
                DiscountType = DiscountType.Percentage,
                DiscountValue = 30m,
                MaxDiscountAmount = 150m,
                Currency = "USD",
                Status = PromotionStatus.Active,
                ValidFrom = now,
                ValidTo = now.AddDays(7),
                MaxTotalUses = 500,
                ApplicableDays = DayOfWeekFlag.AllDays,
                IsActive = true
            },
            // Weekday special
            new()
            {
                Id = Guid.NewGuid(),
                Code = "WEEKDAY15",
                Name = "Weekday Special",
                Description = "15% off Tuesday-Thursday flights",
                Type = PromotionType.General,
                DiscountType = DiscountType.Percentage,
                DiscountValue = 15m,
                MaxDiscountAmount = 75m,
                Currency = "USD",
                Status = PromotionStatus.Active,
                ValidFrom = now,
                ValidTo = now.AddMonths(6),
                ApplicableDays = DayOfWeekFlag.Tuesday | DayOfWeekFlag.Wednesday | DayOfWeekFlag.Thursday,
                IsActive = true
            },
            // Early bird
            new()
            {
                Id = Guid.NewGuid(),
                Code = "EARLY20",
                Name = "Early Bird Discount",
                Description = "20% off when booking 60+ days ahead",
                Type = PromotionType.EarlyBird,
                DiscountType = DiscountType.Percentage,
                DiscountValue = 20m,
                MaxDiscountAmount = 250m,
                MinBookingAmount = 200m,
                Currency = "USD",
                Status = PromotionStatus.Active,
                ValidFrom = now,
                ValidTo = now.AddMonths(12),
                ApplicableDays = DayOfWeekFlag.AllDays,
                IsActive = true
            }
        };

        await _context.Set<Promotion>().AddRangeAsync(promotions);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} promotions", promotions.Count);
    }
}

