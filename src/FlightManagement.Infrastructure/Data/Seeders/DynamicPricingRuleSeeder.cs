using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds dynamic pricing rules.
/// </summary>
public class DynamicPricingRuleSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DynamicPricingRuleSeeder> _logger;

    public DynamicPricingRuleSeeder(ApplicationDbContext context, ILogger<DynamicPricingRuleSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Set<DynamicPricingRule>().AnyAsync())
        {
            _logger.LogInformation("Dynamic pricing rules already seeded");
            return;
        }

        var rules = new List<DynamicPricingRule>
        {
            // Weekend surcharge
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Weekend Surcharge",
                Description = "Price increase for Friday-Sunday travel",
                RuleType = PricingRuleType.DayOfWeek,
                AdjustmentPercentage = 15m,
                ApplicableDays = DayOfWeekFlag.Friday | DayOfWeekFlag.Saturday | DayOfWeekFlag.Sunday,
                Priority = 100,
                IsActive = true
            },
            // High demand pricing (70-85% booked)
            new()
            {
                Id = Guid.NewGuid(),
                Name = "High Demand Pricing",
                Description = "Increase prices when flight is 70-85% full",
                RuleType = PricingRuleType.DemandBased,
                AdjustmentPercentage = 20m,
                MinBookingPercentage = 70m,
                MaxBookingPercentage = 85m,
                Priority = 90,
                IsActive = true
            },
            // Very high demand pricing (85%+ booked)
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Very High Demand Pricing",
                Description = "Significant price increase when flight is >85% full",
                RuleType = PricingRuleType.DemandBased,
                AdjustmentPercentage = 35m,
                MinBookingPercentage = 85m,
                MaxBookingPercentage = 100m,
                Priority = 95,
                IsActive = true
            },
            // Early bird discount (60+ days)
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Early Bird Discount",
                Description = "Discount for bookings made 60+ days in advance",
                RuleType = PricingRuleType.AdvancePurchase,
                AdjustmentPercentage = -15m,
                MinDaysBeforeDeparture = 60,
                MaxDaysBeforeDeparture = 365,
                Priority = 80,
                IsActive = true
            },
            // Standard advance (30-60 days)
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Advance Purchase Discount",
                Description = "Modest discount for 30-60 day advance bookings",
                RuleType = PricingRuleType.AdvancePurchase,
                AdjustmentPercentage = -8m,
                MinDaysBeforeDeparture = 30,
                MaxDaysBeforeDeparture = 59,
                Priority = 75,
                IsActive = true
            },
            // Last minute surcharge (0-3 days)
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Last Minute Surcharge",
                Description = "Premium for bookings within 3 days of departure",
                RuleType = PricingRuleType.LastMinute,
                AdjustmentPercentage = 25m,
                MinDaysBeforeDeparture = 0,
                MaxDaysBeforeDeparture = 3,
                Priority = 110,
                IsActive = true
            },
            // Red-eye discount (late night departures)
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Red-Eye Discount",
                Description = "Discount for late night departures (11PM-5AM)",
                RuleType = PricingRuleType.TimeOfDay,
                AdjustmentPercentage = -12m,
                StartHour = 23,
                EndHour = 5,
                Priority = 70,
                IsActive = true
            },
            // Morning rush premium
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Morning Rush Premium",
                Description = "Premium for popular morning departure slots",
                RuleType = PricingRuleType.TimeOfDay,
                AdjustmentPercentage = 10m,
                StartHour = 7,
                EndHour = 9,
                Priority = 65,
                IsActive = true
            }
        };

        await _context.Set<DynamicPricingRule>().AddRangeAsync(rules);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} dynamic pricing rules", rules.Count);
    }
}

