using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the FlightPricing entity.
/// Manages pricing per cabin class per flight.
/// </summary>
public class FlightPricingConfiguration : IEntityTypeConfiguration<FlightPricing>
{
    public void Configure(EntityTypeBuilder<FlightPricing> builder)
    {
        builder.ToTable("FlightPricings");

        builder.HasKey(fp => fp.Id);

        // Cabin class stored as string
        builder.Property(fp => fp.CabinClass)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Price fields with precision
        builder.Property(fp => fp.BasePrice)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(fp => fp.CurrentPrice)
            .HasPrecision(10, 2)
            .IsRequired();

        builder.Property(fp => fp.TaxAmount)
            .HasPrecision(10, 2)
            .IsRequired();

        // Currency code
        builder.Property(fp => fp.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .HasDefaultValue("USD");

        // Relationship: Pricing belongs to Flight
        builder.HasOne(fp => fp.Flight)
            .WithMany(f => f.FlightPricings)
            .HasForeignKey(fp => fp.FlightId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint: One pricing per class per flight
        builder.HasIndex(fp => new { fp.FlightId, fp.CabinClass })
            .IsUnique();

        // Index for available seats queries
        builder.HasIndex(fp => fp.AvailableSeats);

        // Index for active pricing
        builder.HasIndex(fp => fp.IsActive);

        // Ignore calculated properties (not stored in DB)
        builder.Ignore(fp => fp.TotalPrice);
        builder.Ignore(fp => fp.BookingPercentage);
    }
}

