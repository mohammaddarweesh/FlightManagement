using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds sample flights with pricing and seat availability.
/// </summary>
public class FlightSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<FlightSeeder> _logger;

    public FlightSeeder(ApplicationDbContext context, ILogger<FlightSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Flights.AnyAsync())
        {
            _logger.LogInformation("Flights already seeded");
            return;
        }

        var airlines = await _context.Airlines.ToListAsync();
        var airports = await _context.Airports.ToListAsync();
        var aircraft = await _context.Aircraft.Include(a => a.CabinClasses).ToListAsync();
        var amenities = await _context.Amenities.ToListAsync();

        if (!airlines.Any() || !airports.Any() || !aircraft.Any())
        {
            _logger.LogWarning("Missing required reference data. Skipping flight seeding.");
            return;
        }

        var flights = new List<Flight>();
        var random = new Random(42); // Fixed seed for reproducibility

        // Create flights for the next 30 days
        for (int dayOffset = 1; dayOffset <= 30; dayOffset++)
        {
            var date = DateTime.UtcNow.Date.AddDays(dayOffset);

            // Create multiple flights per day
            foreach (var airline in airlines.Take(6)) // Use first 6 airlines
            {
                var airlineAircraft = aircraft.Where(a => a.AirlineId == airline.Id).ToList();
                if (!airlineAircraft.Any()) continue;

                // Create 2-3 routes per airline per day
                var routes = GetPopularRoutes(airports).Take(random.Next(2, 4));

                foreach (var (departure, arrival) in routes)
                {
                    var selectedAircraft = airlineAircraft[random.Next(airlineAircraft.Count)];
                    var departureHour = random.Next(6, 22);
                    var flightDuration = TimeSpan.FromHours(random.Next(2, 8) + random.NextDouble());

                    var flight = CreateFlight(
                        airline, selectedAircraft, departure, arrival,
                        date.AddHours(departureHour), flightDuration, amenities, random);

                    flights.Add(flight);
                }
            }
        }

        await _context.Flights.AddRangeAsync(flights);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} flights", flights.Count);
    }

    private static IEnumerable<(Airport, Airport)> GetPopularRoutes(List<Airport> airports)
    {
        var jfk = airports.FirstOrDefault(a => a.IataCode == "JFK");
        var lax = airports.FirstOrDefault(a => a.IataCode == "LAX");
        var lhr = airports.FirstOrDefault(a => a.IataCode == "LHR");
        var cdg = airports.FirstOrDefault(a => a.IataCode == "CDG");
        var dxb = airports.FirstOrDefault(a => a.IataCode == "DXB");
        var sin = airports.FirstOrDefault(a => a.IataCode == "SIN");
        var sfo = airports.FirstOrDefault(a => a.IataCode == "SFO");
        var ord = airports.FirstOrDefault(a => a.IataCode == "ORD");

        var routes = new List<(Airport?, Airport?)>
        {
            (jfk, lax), (lax, jfk), (jfk, lhr), (lhr, jfk),
            (lax, lhr), (lhr, cdg), (cdg, dxb), (dxb, sin),
            (jfk, cdg), (sfo, jfk), (ord, lax), (lhr, sin)
        };

        return routes.Where(r => r.Item1 != null && r.Item2 != null)
                     .Select(r => (r.Item1!, r.Item2!));
    }

    private Flight CreateFlight(Airline airline, Aircraft ac, Airport dep, Airport arr,
        DateTime depTime, TimeSpan duration, List<Amenity> amenities, Random random)
    {
        var flight = new Flight
        {
            Id = Guid.NewGuid(),
            FlightNumber = $"{airline.IataCode}{random.Next(100, 999)}",
            AirlineId = airline.Id,
            AircraftId = ac.Id,
            DepartureAirportId = dep.Id,
            ArrivalAirportId = arr.Id,
            ScheduledDepartureTime = depTime,
            ScheduledArrivalTime = depTime.Add(duration),
            Duration = duration,
            DepartureTerminal = $"T{random.Next(1, 5)}",
            DepartureGate = $"{(char)('A' + random.Next(0, 6))}{random.Next(1, 30)}",
            Status = FlightStatus.Scheduled,
            IsActive = true
        };

        // Add pricing for each cabin class
        foreach (var cabin in ac.CabinClasses)
        {
            flight.FlightPricings.Add(new FlightPricing
            {
                Id = Guid.NewGuid(),
                FlightId = flight.Id,
                CabinClass = cabin.CabinClass,
                BasePrice = GetBasePrice(cabin.CabinClass, duration, random),
                Currency = "USD",
                AvailableSeats = cabin.SeatCount,
                IsActive = true
            });
        }

        // Add some amenities
        foreach (var amenity in amenities.OrderBy(_ => random.Next()).Take(random.Next(3, 6)))
        {
            var isIncluded = random.NextDouble() > 0.3;
            flight.FlightAmenities.Add(new FlightAmenity
            {
                Id = Guid.NewGuid(),
                FlightId = flight.Id,
                AmenityId = amenity.Id,
                IsIncluded = isIncluded,
                Price = isIncluded ? null : random.Next(10, 50),
                Currency = isIncluded ? null : "USD"
            });
        }

        return flight;
    }

    private static decimal GetBasePrice(FlightClass cabin, TimeSpan duration, Random random)
    {
        var hours = duration.TotalHours;
        var baseMultiplier = cabin switch
        {
            FlightClass.First => 8m,
            FlightClass.Business => 4m,
            FlightClass.PremiumEconomy => 1.8m,
            _ => 1m
        };
        return Math.Round((decimal)(hours * 50 * (double)baseMultiplier * (0.8 + random.NextDouble() * 0.4)), 2);
    }
}

