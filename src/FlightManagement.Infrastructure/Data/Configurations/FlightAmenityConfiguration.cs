using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the FlightAmenity entity.
/// Links amenities to flights with class-specific settings.
/// </summary>
public class FlightAmenityConfiguration : IEntityTypeConfiguration<FlightAmenity>
{
    public void Configure(EntityTypeBuilder<FlightAmenity> builder)
    {
        builder.ToTable("FlightAmenities");

        builder.HasKey(fa => fa.Id);

        // Cabin class stored as string (nullable)
        builder.Property(fa => fa.CabinClass)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Price for purchasable amenities
        builder.Property(fa => fa.Price)
            .HasPrecision(10, 2);

        // Currency code
        builder.Property(fa => fa.Currency)
            .HasMaxLength(3);

        // Relationship: FlightAmenity belongs to Flight
        builder.HasOne(fa => fa.Flight)
            .WithMany(f => f.FlightAmenities)
            .HasForeignKey(fa => fa.FlightId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship: FlightAmenity references Amenity
        builder.HasOne(fa => fa.Amenity)
            .WithMany(a => a.FlightAmenities)
            .HasForeignKey(fa => fa.AmenityId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraint: Prevent duplicate amenity per flight per class
        // Note: CabinClass can be null (applies to all classes)
        builder.HasIndex(fa => new { fa.FlightId, fa.AmenityId, fa.CabinClass })
            .IsUnique();

        // Index for flight amenities lookup
        builder.HasIndex(fa => fa.FlightId);

        // Index for included amenities
        builder.HasIndex(fa => fa.IsIncluded);
    }
}

