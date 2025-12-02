using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds booking policies.
/// </summary>
public class BookingPolicySeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BookingPolicySeeder> _logger;

    public BookingPolicySeeder(ApplicationDbContext context, ILogger<BookingPolicySeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Set<BookingPolicy>().AnyAsync())
        {
            _logger.LogInformation("Booking policies already seeded");
            return;
        }

        var policies = new List<BookingPolicy>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Code = "MIN_ADV_2H",
                Name = "Minimum 2 Hours Advance",
                Description = "Bookings must be made at least 2 hours before departure",
                Type = PolicyType.MinimumAdvancePurchase,
                Value = 2,
                ErrorMessage = "Bookings must be made at least 2 hours before departure",
                Priority = 100,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Code = "MAX_ADV_365D",
                Name = "Maximum 365 Days Advance",
                Description = "Bookings can be made up to 365 days in advance",
                Type = PolicyType.MaximumAdvanceBooking,
                Value = 365 * 24, // 365 days in hours
                ErrorMessage = "Bookings cannot be made more than 365 days in advance",
                Priority = 90,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Code = "MAX_PAX_9",
                Name = "Maximum 9 Passengers",
                Description = "Maximum of 9 passengers per booking",
                Type = PolicyType.MaximumPassengers,
                Value = 9,
                ErrorMessage = "Maximum of 9 passengers allowed per booking. For larger groups, please contact our group booking team.",
                Priority = 80,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Code = "MIN_PAX_1",
                Name = "Minimum 1 Passenger",
                Description = "At least 1 passenger required",
                Type = PolicyType.MinimumPassengers,
                Value = 1,
                ErrorMessage = "At least one passenger is required for booking",
                Priority = 80,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Code = "INFANT_AGE",
                Name = "Infant Age Policy",
                Description = "Infants must be under 2 years old at time of travel",
                Type = PolicyType.AgeRestriction,
                Value = 0,
                SecondaryValue = 2,
                ErrorMessage = "Infants must be under 2 years of age at time of travel",
                Priority = 70,
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Code = "CHILD_AGE",
                Name = "Child Age Policy",
                Description = "Children must be between 2 and 12 years old",
                Type = PolicyType.AgeRestriction,
                Value = 2,
                SecondaryValue = 12,
                ErrorMessage = "Children must be between 2 and 12 years of age",
                Priority = 70,
                IsActive = true
            }
        };

        await _context.Set<BookingPolicy>().AddRangeAsync(policies);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} booking policies", policies.Count);
    }
}

