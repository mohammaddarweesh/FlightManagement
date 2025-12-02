using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds the master list of available amenities.
/// </summary>
public class AmenitySeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AmenitySeeder> _logger;

    public AmenitySeeder(ApplicationDbContext context, ILogger<AmenitySeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Amenities.AnyAsync())
        {
            _logger.LogInformation("Amenities already seeded");
            return;
        }

        var amenities = new List<Amenity>
        {
            // Connectivity
            new() { Id = Guid.NewGuid(), Code = "WIFI", Name = "In-Flight WiFi", Description = "High-speed internet access during the flight", Category = AmenityCategory.Connectivity },
            new() { Id = Guid.NewGuid(), Code = "POWER", Name = "Power Outlet", Description = "AC power outlet at every seat", Category = AmenityCategory.Connectivity },
            new() { Id = Guid.NewGuid(), Code = "USB", Name = "USB Charging", Description = "USB charging ports at every seat", Category = AmenityCategory.Connectivity },
            
            // Entertainment
            new() { Id = Guid.NewGuid(), Code = "IFE", Name = "Seatback Entertainment", Description = "Personal screen with movies, TV shows, and games", Category = AmenityCategory.Entertainment },
            new() { Id = Guid.NewGuid(), Code = "STREAM", Name = "Streaming Entertainment", Description = "Stream content to your personal device", Category = AmenityCategory.Entertainment },
            new() { Id = Guid.NewGuid(), Code = "HEADPHONES", Name = "Noise-Canceling Headphones", Description = "Premium noise-canceling headphones provided", Category = AmenityCategory.Entertainment },
            
            // Comfort
            new() { Id = Guid.NewGuid(), Code = "LEGROOM", Name = "Extra Legroom", Description = "Additional legroom for more comfort", Category = AmenityCategory.Comfort },
            new() { Id = Guid.NewGuid(), Code = "BLANKET", Name = "Blanket & Pillow", Description = "Complimentary blanket and pillow", Category = AmenityCategory.Comfort },
            new() { Id = Guid.NewGuid(), Code = "AMENITY_KIT", Name = "Amenity Kit", Description = "Premium amenity kit with toiletries", Category = AmenityCategory.Comfort },
            new() { Id = Guid.NewGuid(), Code = "LIE_FLAT", Name = "Lie-Flat Seat", Description = "Seat converts to fully flat bed", Category = AmenityCategory.Comfort },
            new() { Id = Guid.NewGuid(), Code = "PAJAMAS", Name = "Sleepwear", Description = "Pajamas provided for overnight flights", Category = AmenityCategory.Comfort },
            
            // Dining
            new() { Id = Guid.NewGuid(), Code = "MEAL", Name = "Complimentary Meal", Description = "Full meal service included", Category = AmenityCategory.Dining },
            new() { Id = Guid.NewGuid(), Code = "SNACK", Name = "Snacks & Beverages", Description = "Complimentary snacks and non-alcoholic beverages", Category = AmenityCategory.Dining },
            new() { Id = Guid.NewGuid(), Code = "ALCOHOL", Name = "Alcoholic Beverages", Description = "Complimentary wine, beer, and spirits", Category = AmenityCategory.Dining },
            new() { Id = Guid.NewGuid(), Code = "PREMIUM_DINING", Name = "Premium Dining", Description = "Multi-course gourmet dining experience", Category = AmenityCategory.Dining },
            new() { Id = Guid.NewGuid(), Code = "CHEF", Name = "On-Demand Dining", Description = "Dine when you want with on-demand service", Category = AmenityCategory.Dining },
            
            // Baggage
            new() { Id = Guid.NewGuid(), Code = "CARRY_ON", Name = "Carry-On Bag", Description = "Personal item and carry-on bag included", Category = AmenityCategory.Baggage },
            new() { Id = Guid.NewGuid(), Code = "CHECKED_1", Name = "1 Checked Bag", Description = "One checked bag up to 23kg included", Category = AmenityCategory.Baggage },
            new() { Id = Guid.NewGuid(), Code = "CHECKED_2", Name = "2 Checked Bags", Description = "Two checked bags up to 23kg each included", Category = AmenityCategory.Baggage },
            new() { Id = Guid.NewGuid(), Code = "PRIORITY_BAG", Name = "Priority Baggage", Description = "Your bags arrive first at baggage claim", Category = AmenityCategory.Baggage },
            
            // Priority Services
            new() { Id = Guid.NewGuid(), Code = "PRIORITY_BOARD", Name = "Priority Boarding", Description = "Board the aircraft before general boarding", Category = AmenityCategory.Priority },
            new() { Id = Guid.NewGuid(), Code = "PRIORITY_CHECK", Name = "Priority Check-In", Description = "Dedicated priority check-in counter", Category = AmenityCategory.Priority },
            new() { Id = Guid.NewGuid(), Code = "FAST_TRACK", Name = "Fast Track Security", Description = "Expedited security screening", Category = AmenityCategory.Priority },
            new() { Id = Guid.NewGuid(), Code = "LOUNGE", Name = "Lounge Access", Description = "Access to airport lounge before departure", Category = AmenityCategory.Priority },
        };

        await _context.Amenities.AddRangeAsync(amenities);
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Seeded {Count} amenities", amenities.Count);
    }
}

