using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds blackout dates for promotions and bookings.
/// </summary>
public class BlackoutDateSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BlackoutDateSeeder> _logger;

    public BlackoutDateSeeder(ApplicationDbContext context, ILogger<BlackoutDateSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Set<BlackoutDate>().AnyAsync())
        {
            _logger.LogInformation("Blackout dates already seeded");
            return;
        }

        var currentYear = DateTime.UtcNow.Year;
        var nextYear = currentYear + 1;

        var blackoutDates = new List<BlackoutDate>
        {
            // Christmas Peak - No promos
            new()
            {
                Id = Guid.NewGuid(),
                Name = $"Christmas {currentYear} Promo Blackout",
                Description = "No promotional codes during Christmas peak",
                StartDate = new DateTime(currentYear, 12, 20, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(currentYear, 12, 31, 23, 59, 59, DateTimeKind.Utc),
                BlocksBookings = false,
                BlocksPromotions = true,
                IsActive = true
            },
            // New Year - No promos
            new()
            {
                Id = Guid.NewGuid(),
                Name = $"New Year {nextYear} Promo Blackout",
                Description = "No promotional codes during New Year",
                StartDate = new DateTime(nextYear, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(nextYear, 1, 3, 23, 59, 59, DateTimeKind.Utc),
                BlocksBookings = false,
                BlocksPromotions = true,
                IsActive = true
            },
            // Thanksgiving - No promos
            new()
            {
                Id = Guid.NewGuid(),
                Name = $"Thanksgiving {currentYear} Promo Blackout",
                Description = "No promotional codes during Thanksgiving",
                StartDate = new DateTime(currentYear, 11, 22, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(currentYear, 11, 28, 23, 59, 59, DateTimeKind.Utc),
                BlocksBookings = false,
                BlocksPromotions = true,
                IsActive = true
            },
            // Spring Break Peak - No promos
            new()
            {
                Id = Guid.NewGuid(),
                Name = $"Spring Break {nextYear} Promo Blackout",
                Description = "No promotional codes during Spring Break",
                StartDate = new DateTime(nextYear, 3, 15, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(nextYear, 3, 31, 23, 59, 59, DateTimeKind.Utc),
                BlocksBookings = false,
                BlocksPromotions = true,
                IsActive = true
            },
            // July 4th Weekend
            new()
            {
                Id = Guid.NewGuid(),
                Name = $"July 4th {nextYear} Promo Blackout",
                Description = "No promotional codes during Independence Day weekend",
                StartDate = new DateTime(nextYear, 7, 2, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(nextYear, 7, 6, 23, 59, 59, DateTimeKind.Utc),
                BlocksBookings = false,
                BlocksPromotions = true,
                IsActive = true
            },
            // Labor Day Weekend
            new()
            {
                Id = Guid.NewGuid(),
                Name = $"Labor Day {nextYear} Promo Blackout",
                Description = "No promotional codes during Labor Day weekend",
                StartDate = new DateTime(nextYear, 8, 30, 0, 0, 0, DateTimeKind.Utc),
                EndDate = new DateTime(nextYear, 9, 3, 23, 59, 59, DateTimeKind.Utc),
                BlocksBookings = false,
                BlocksPromotions = true,
                IsActive = true
            }
        };

        await _context.Set<BlackoutDate>().AddRangeAsync(blackoutDates);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} blackout dates", blackoutDates.Count);
    }
}

