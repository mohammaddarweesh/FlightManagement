using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds major international airlines.
/// </summary>
public class AirlineSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AirlineSeeder> _logger;

    public AirlineSeeder(ApplicationDbContext context, ILogger<AirlineSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Airlines.AnyAsync())
        {
            _logger.LogInformation("Airlines already seeded");
            return;
        }

        var airlines = new List<Airline>
        {
            // US Airlines
            new() { Id = Guid.NewGuid(), IataCode = "AA", IcaoCode = "AAL", Name = "American Airlines", Country = "United States" },
            new() { Id = Guid.NewGuid(), IataCode = "UA", IcaoCode = "UAL", Name = "United Airlines", Country = "United States" },
            new() { Id = Guid.NewGuid(), IataCode = "DL", IcaoCode = "DAL", Name = "Delta Air Lines", Country = "United States" },
            
            // European Airlines
            new() { Id = Guid.NewGuid(), IataCode = "BA", IcaoCode = "BAW", Name = "British Airways", Country = "United Kingdom" },
            new() { Id = Guid.NewGuid(), IataCode = "AF", IcaoCode = "AFR", Name = "Air France", Country = "France" },
            new() { Id = Guid.NewGuid(), IataCode = "LH", IcaoCode = "DLH", Name = "Lufthansa", Country = "Germany" },
            new() { Id = Guid.NewGuid(), IataCode = "KL", IcaoCode = "KLM", Name = "KLM Royal Dutch Airlines", Country = "Netherlands" },
            
            // Middle East Airlines
            new() { Id = Guid.NewGuid(), IataCode = "EK", IcaoCode = "UAE", Name = "Emirates", Country = "United Arab Emirates" },
            new() { Id = Guid.NewGuid(), IataCode = "QR", IcaoCode = "QTR", Name = "Qatar Airways", Country = "Qatar" },
            
            // Asian Airlines
            new() { Id = Guid.NewGuid(), IataCode = "SQ", IcaoCode = "SIA", Name = "Singapore Airlines", Country = "Singapore" },
            new() { Id = Guid.NewGuid(), IataCode = "CX", IcaoCode = "CPA", Name = "Cathay Pacific", Country = "Hong Kong" },
            new() { Id = Guid.NewGuid(), IataCode = "JL", IcaoCode = "JAL", Name = "Japan Airlines", Country = "Japan" },
        };

        await _context.Airlines.AddRangeAsync(airlines);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Seeded {Count} airlines", airlines.Count);
    }
}

