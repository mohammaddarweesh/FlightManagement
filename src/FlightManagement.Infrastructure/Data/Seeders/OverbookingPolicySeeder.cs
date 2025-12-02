using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds overbooking policies for airlines.
/// </summary>
public class OverbookingPolicySeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OverbookingPolicySeeder> _logger;

    public OverbookingPolicySeeder(ApplicationDbContext context, ILogger<OverbookingPolicySeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Set<OverbookingPolicy>().AnyAsync())
        {
            _logger.LogInformation("Overbooking policies already seeded");
            return;
        }

        var airlines = await _context.Airlines.ToListAsync();
        if (!airlines.Any())
        {
            _logger.LogWarning("No airlines found. Skipping overbooking policy seeding.");
            return;
        }

        var policies = new List<OverbookingPolicy>();

        foreach (var airline in airlines)
        {
            // Economy class - higher overbooking allowed
            policies.Add(new OverbookingPolicy
            {
                Id = Guid.NewGuid(),
                AirlineId = airline.Id,
                Name = $"{airline.IataCode} Economy Overbooking",
                Description = $"Overbooking policy for {airline.Name} Economy class",
                MaxOverbookingPercentage = 5m,
                MaxOverbookedSeats = 10,
                CabinClass = FlightClass.Economy,
                Priority = 100,
                IsActive = true
            });

            // Business class - limited overbooking
            policies.Add(new OverbookingPolicy
            {
                Id = Guid.NewGuid(),
                AirlineId = airline.Id,
                Name = $"{airline.IataCode} Business Overbooking",
                Description = $"Overbooking policy for {airline.Name} Business class",
                MaxOverbookingPercentage = 2m,
                MaxOverbookedSeats = 2,
                CabinClass = FlightClass.Business,
                Priority = 100,
                IsActive = true
            });

            // First class - no overbooking
            policies.Add(new OverbookingPolicy
            {
                Id = Guid.NewGuid(),
                AirlineId = airline.Id,
                Name = $"{airline.IataCode} First Class Overbooking",
                Description = $"No overbooking for {airline.Name} First class",
                MaxOverbookingPercentage = 0m,
                MaxOverbookedSeats = 0,
                CabinClass = FlightClass.First,
                Priority = 100,
                IsActive = true
            });
        }

        await _context.Set<OverbookingPolicy>().AddRangeAsync(policies);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} overbooking policies for {AirlineCount} airlines", 
            policies.Count, airlines.Count);
    }
}

