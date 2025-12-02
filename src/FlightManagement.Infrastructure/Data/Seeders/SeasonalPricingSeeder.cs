using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds seasonal pricing periods.
/// </summary>
public class SeasonalPricingSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SeasonalPricingSeeder> _logger;

    public SeasonalPricingSeeder(ApplicationDbContext context, ILogger<SeasonalPricingSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Set<SeasonalPricing>().AnyAsync())
        {
            _logger.LogInformation("Seasonal pricing already seeded");
            return;
        }

        var currentYear = DateTime.UtcNow.Year;
        var nextYear = currentYear + 1;

        var seasonalPricings = new List<SeasonalPricing>
        {
            // Summer Peak Season
            new()
            {
                Id = Guid.NewGuid(),
                Name = $"Summer Peak {currentYear}",
                Description = "Peak summer travel season with increased demand",
                SeasonType = SeasonType.Peak,
                StartDate = new DateTime(currentYear, 6, 15, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(currentYear, 8, 31, 23, 59, 59, DateTimeKind.Utc),
                AdjustmentPercentage = 25m,
                Priority = 100,
                IsActive = true
            },
            // Christmas/New Year Peak
            new()
            {
                Id = Guid.NewGuid(),
                Name = $"Christmas Peak {currentYear}",
                Description = "Holiday season peak pricing",
                SeasonType = SeasonType.Holiday,
                StartDate = new DateTime(currentYear, 12, 18, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(nextYear, 1, 5, 23, 59, 59, DateTimeKind.Utc),
                AdjustmentPercentage = 35m,
                Priority = 110,
                IsActive = true
            },
            // Thanksgiving Peak
            new()
            {
                Id = Guid.NewGuid(),
                Name = $"Thanksgiving {currentYear}",
                Description = "Thanksgiving travel period",
                SeasonType = SeasonType.Holiday,
                StartDate = new DateTime(currentYear, 11, 20, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(currentYear, 11, 30, 23, 59, 59, DateTimeKind.Utc),
                AdjustmentPercentage = 30m,
                Priority = 105,
                IsActive = true
            },
            // Spring Break
            new()
            {
                Id = Guid.NewGuid(),
                Name = $"Spring Break {nextYear}",
                Description = "Spring break travel surge",
                SeasonType = SeasonType.Peak,
                StartDate = new DateTime(nextYear, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(nextYear, 4, 5, 23, 59, 59, DateTimeKind.Utc),
                AdjustmentPercentage = 20m,
                Priority = 95,
                IsActive = true
            },
            // Low Season - January-February
            new()
            {
                Id = Guid.NewGuid(),
                Name = $"Winter Low Season {nextYear}",
                Description = "Post-holiday low demand period",
                SeasonType = SeasonType.Low,
                StartDate = new DateTime(nextYear, 1, 10, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(nextYear, 2, 28, 23, 59, 59, DateTimeKind.Utc),
                AdjustmentPercentage = -15m,
                Priority = 80,
                IsActive = true
            },
            // Low Season - September-October
            new()
            {
                Id = Guid.NewGuid(),
                Name = $"Fall Low Season {currentYear}",
                Description = "Back to school low demand period",
                SeasonType = SeasonType.Low,
                StartDate = new DateTime(currentYear, 9, 5, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(currentYear, 10, 15, 23, 59, 59, DateTimeKind.Utc),
                AdjustmentPercentage = -10m,
                Priority = 75,
                IsActive = true
            }
        };

        await _context.Set<SeasonalPricing>().AddRangeAsync(seasonalPricings);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} seasonal pricing periods", seasonalPricings.Count);
    }
}

