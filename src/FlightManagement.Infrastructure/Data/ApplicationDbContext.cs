using System.Reflection;
using FlightManagement.Domain.Common;
using FlightManagement.Domain.Entities;
using FlightManagement.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlightManagement.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    private readonly ICurrentUserService _currentUserService;

    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,
        ICurrentUserService currentUserService) : base(options)
    {
        _currentUserService = currentUserService;
    }

    // User & Customer
    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();

    // Flight Management
    public DbSet<Airport> Airports => Set<Airport>();
    public DbSet<Airline> Airlines => Set<Airline>();
    public DbSet<Aircraft> Aircraft => Set<Aircraft>();
    public DbSet<AircraftCabinClass> AircraftCabinClasses => Set<AircraftCabinClass>();
    public DbSet<Seat> Seats => Set<Seat>();
    public DbSet<Flight> Flights => Set<Flight>();
    public DbSet<FlightPricing> FlightPricings => Set<FlightPricing>();
    public DbSet<FlightSeat> FlightSeats => Set<FlightSeat>();
    public DbSet<Amenity> Amenities => Set<Amenity>();
    public DbSet<FlightAmenity> FlightAmenities => Set<FlightAmenity>();

    // Booking Management
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Passenger> Passengers => Set<Passenger>();
    public DbSet<BookingSegment> BookingSegments => Set<BookingSegment>();
    public DbSet<PassengerSeat> PassengerSeats => Set<PassengerSeat>();
    public DbSet<BookingExtra> BookingExtras => Set<BookingExtra>();
    public DbSet<PaymentRecord> PaymentRecords => Set<PaymentRecord>();
    public DbSet<CancellationPolicy> CancellationPolicies => Set<CancellationPolicy>();
    public DbSet<CancellationPolicyRule> CancellationPolicyRules => Set<CancellationPolicyRule>();
    public DbSet<BookingHistory> BookingHistories => Set<BookingHistory>();

    // Promotions & Pricing
    public DbSet<Promotion> Promotions => Set<Promotion>();
    public DbSet<PromotionUsage> PromotionUsages => Set<PromotionUsage>();
    public DbSet<DynamicPricingRule> DynamicPricingRules => Set<DynamicPricingRule>();
    public DbSet<SeasonalPricing> SeasonalPricings => Set<SeasonalPricing>();

    // Policies
    public DbSet<BookingPolicy> BookingPolicies => Set<BookingPolicy>();
    public DbSet<BlackoutDate> BlackoutDates => Set<BlackoutDate>();
    public DbSet<OverbookingPolicy> OverbookingPolicies => Set<OverbookingPolicy>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Handle audit fields
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.CreatedBy = _currentUserService.UserName;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = _currentUserService.UserName;
                    break;
            }
        }

        // Handle soft delete
        foreach (var entry in ChangeTracker.Entries<ISoftDeletable>())
        {
            if (entry.State == EntityState.Deleted)
            {
                entry.State = EntityState.Modified;
                entry.Entity.IsDeleted = true;
                entry.Entity.DeletedAt = DateTime.UtcNow;
                entry.Entity.DeletedBy = _currentUserService.UserName;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}

