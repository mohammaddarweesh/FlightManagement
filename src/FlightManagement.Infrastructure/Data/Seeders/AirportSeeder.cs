using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds major international airports.
/// </summary>
public class AirportSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AirportSeeder> _logger;

    public AirportSeeder(ApplicationDbContext context, ILogger<AirportSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Airports.AnyAsync())
        {
            _logger.LogInformation("Airports already seeded");
            return;
        }

        var airports = new List<Airport>
        {
            // United States
            new() { Id = Guid.NewGuid(), IataCode = "JFK", IcaoCode = "KJFK", Name = "John F. Kennedy International Airport", City = "New York", Country = "United States", CountryCode = "US", Timezone = "America/New_York", Latitude = 40.6413m, Longitude = -73.7781m },
            new() { Id = Guid.NewGuid(), IataCode = "LAX", IcaoCode = "KLAX", Name = "Los Angeles International Airport", City = "Los Angeles", Country = "United States", CountryCode = "US", Timezone = "America/Los_Angeles", Latitude = 33.9425m, Longitude = -118.4081m },
            new() { Id = Guid.NewGuid(), IataCode = "ORD", IcaoCode = "KORD", Name = "O'Hare International Airport", City = "Chicago", Country = "United States", CountryCode = "US", Timezone = "America/Chicago", Latitude = 41.9742m, Longitude = -87.9073m },
            new() { Id = Guid.NewGuid(), IataCode = "DFW", IcaoCode = "KDFW", Name = "Dallas/Fort Worth International Airport", City = "Dallas", Country = "United States", CountryCode = "US", Timezone = "America/Chicago", Latitude = 32.8998m, Longitude = -97.0403m },
            new() { Id = Guid.NewGuid(), IataCode = "MIA", IcaoCode = "KMIA", Name = "Miami International Airport", City = "Miami", Country = "United States", CountryCode = "US", Timezone = "America/New_York", Latitude = 25.7959m, Longitude = -80.2870m },
            
            // Europe
            new() { Id = Guid.NewGuid(), IataCode = "LHR", IcaoCode = "EGLL", Name = "London Heathrow Airport", City = "London", Country = "United Kingdom", CountryCode = "GB", Timezone = "Europe/London", Latitude = 51.4700m, Longitude = -0.4543m },
            new() { Id = Guid.NewGuid(), IataCode = "CDG", IcaoCode = "LFPG", Name = "Paris Charles de Gaulle Airport", City = "Paris", Country = "France", CountryCode = "FR", Timezone = "Europe/Paris", Latitude = 49.0097m, Longitude = 2.5479m },
            new() { Id = Guid.NewGuid(), IataCode = "FRA", IcaoCode = "EDDF", Name = "Frankfurt Airport", City = "Frankfurt", Country = "Germany", CountryCode = "DE", Timezone = "Europe/Berlin", Latitude = 50.0379m, Longitude = 8.5622m },
            new() { Id = Guid.NewGuid(), IataCode = "AMS", IcaoCode = "EHAM", Name = "Amsterdam Schiphol Airport", City = "Amsterdam", Country = "Netherlands", CountryCode = "NL", Timezone = "Europe/Amsterdam", Latitude = 52.3105m, Longitude = 4.7683m },
            
            // Middle East
            new() { Id = Guid.NewGuid(), IataCode = "DXB", IcaoCode = "OMDB", Name = "Dubai International Airport", City = "Dubai", Country = "United Arab Emirates", CountryCode = "AE", Timezone = "Asia/Dubai", Latitude = 25.2532m, Longitude = 55.3657m },
            new() { Id = Guid.NewGuid(), IataCode = "DOH", IcaoCode = "OTHH", Name = "Hamad International Airport", City = "Doha", Country = "Qatar", CountryCode = "QA", Timezone = "Asia/Qatar", Latitude = 25.2731m, Longitude = 51.6081m },
            
            // Asia
            new() { Id = Guid.NewGuid(), IataCode = "SIN", IcaoCode = "WSSS", Name = "Singapore Changi Airport", City = "Singapore", Country = "Singapore", CountryCode = "SG", Timezone = "Asia/Singapore", Latitude = 1.3644m, Longitude = 103.9915m },
            new() { Id = Guid.NewGuid(), IataCode = "HKG", IcaoCode = "VHHH", Name = "Hong Kong International Airport", City = "Hong Kong", Country = "Hong Kong", CountryCode = "HK", Timezone = "Asia/Hong_Kong", Latitude = 22.3080m, Longitude = 113.9185m },
            new() { Id = Guid.NewGuid(), IataCode = "NRT", IcaoCode = "RJAA", Name = "Narita International Airport", City = "Tokyo", Country = "Japan", CountryCode = "JP", Timezone = "Asia/Tokyo", Latitude = 35.7720m, Longitude = 140.3929m },
        };

        await _context.Airports.AddRangeAsync(airports);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Seeded {Count} airports", airports.Count);
    }
}

