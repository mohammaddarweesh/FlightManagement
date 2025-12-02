using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Master seeder that orchestrates all database seeders in the correct order.
/// </summary>
public class DatabaseSeeder
{
    private readonly AdminSeeder _adminSeeder;
    private readonly AirportSeeder _airportSeeder;
    private readonly AirlineSeeder _airlineSeeder;
    private readonly AmenitySeeder _amenitySeeder;
    private readonly AircraftSeeder _aircraftSeeder;
    private readonly CancellationPolicySeeder _cancellationPolicySeeder;
    private readonly BookingPolicySeeder _bookingPolicySeeder;
    private readonly OverbookingPolicySeeder _overbookingPolicySeeder;
    private readonly SeasonalPricingSeeder _seasonalPricingSeeder;
    private readonly DynamicPricingRuleSeeder _dynamicPricingRuleSeeder;
    private readonly BlackoutDateSeeder _blackoutDateSeeder;
    private readonly PromotionSeeder _promotionSeeder;
    private readonly FlightSeeder _flightSeeder;
    private readonly CustomerSeeder _customerSeeder;
    private readonly BookingSeeder _bookingSeeder;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(
        AdminSeeder adminSeeder,
        AirportSeeder airportSeeder,
        AirlineSeeder airlineSeeder,
        AmenitySeeder amenitySeeder,
        AircraftSeeder aircraftSeeder,
        CancellationPolicySeeder cancellationPolicySeeder,
        BookingPolicySeeder bookingPolicySeeder,
        OverbookingPolicySeeder overbookingPolicySeeder,
        SeasonalPricingSeeder seasonalPricingSeeder,
        DynamicPricingRuleSeeder dynamicPricingRuleSeeder,
        BlackoutDateSeeder blackoutDateSeeder,
        PromotionSeeder promotionSeeder,
        FlightSeeder flightSeeder,
        CustomerSeeder customerSeeder,
        BookingSeeder bookingSeeder,
        ILogger<DatabaseSeeder> logger)
    {
        _adminSeeder = adminSeeder;
        _airportSeeder = airportSeeder;
        _airlineSeeder = airlineSeeder;
        _amenitySeeder = amenitySeeder;
        _aircraftSeeder = aircraftSeeder;
        _cancellationPolicySeeder = cancellationPolicySeeder;
        _bookingPolicySeeder = bookingPolicySeeder;
        _overbookingPolicySeeder = overbookingPolicySeeder;
        _seasonalPricingSeeder = seasonalPricingSeeder;
        _dynamicPricingRuleSeeder = dynamicPricingRuleSeeder;
        _blackoutDateSeeder = blackoutDateSeeder;
        _promotionSeeder = promotionSeeder;
        _flightSeeder = flightSeeder;
        _customerSeeder = customerSeeder;
        _bookingSeeder = bookingSeeder;
        _logger = logger;
    }

    /// <summary>
    /// Seeds all data in the correct order.
    /// Order matters: Dependencies must exist before dependent entities.
    /// </summary>
    public async Task SeedAllAsync()
    {
        _logger.LogInformation("Starting database seeding...");

        try
        {
            // Phase 1: Core reference data (independent)
            _logger.LogInformation("Seeding core reference data...");
            await _adminSeeder.SeedAsync();
            await _airportSeeder.SeedAsync();
            await _airlineSeeder.SeedAsync();
            await _amenitySeeder.SeedAsync();

            // Phase 2: Aircraft (depends on Airlines)
            _logger.LogInformation("Seeding aircraft...");
            await _aircraftSeeder.SeedAsync();

            // Phase 3: Policies and business rules (depends on Airlines)
            _logger.LogInformation("Seeding policies and business rules...");
            await _cancellationPolicySeeder.SeedAsync();
            await _bookingPolicySeeder.SeedAsync();
            await _overbookingPolicySeeder.SeedAsync();
            await _seasonalPricingSeeder.SeedAsync();
            await _dynamicPricingRuleSeeder.SeedAsync();
            await _blackoutDateSeeder.SeedAsync();
            await _promotionSeeder.SeedAsync();

            // Phase 4: Flights (depends on Airlines, Aircraft, Airports, Amenities)
            _logger.LogInformation("Seeding flights...");
            await _flightSeeder.SeedAsync();

            // Phase 5: Customers (independent, but needed for bookings)
            _logger.LogInformation("Seeding customers...");
            await _customerSeeder.SeedAsync();

            // Phase 6: Bookings (depends on Customers, Flights, CancellationPolicies)
            _logger.LogInformation("Seeding bookings...");
            await _bookingSeeder.SeedAsync();

            _logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during database seeding");
            throw;
        }
    }
}

