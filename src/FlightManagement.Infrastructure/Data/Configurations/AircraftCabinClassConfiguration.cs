using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the AircraftCabinClass entity.
/// Defines cabin configurations per aircraft.
/// </summary>
public class AircraftCabinClassConfiguration : IEntityTypeConfiguration<AircraftCabinClass>
{
    public void Configure(EntityTypeBuilder<AircraftCabinClass> builder)
    {
        builder.ToTable("AircraftCabinClasses");

        builder.HasKey(acc => acc.Id);

        // Cabin class stored as string for readability
        builder.Property(acc => acc.CabinClass)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Seat layout pattern
        builder.Property(acc => acc.SeatLayout)
            .IsRequired()
            .HasMaxLength(20);

        // Price multiplier with precision
        builder.Property(acc => acc.BasePriceMultiplier)
            .HasPrecision(5, 2)
            .HasDefaultValue(1.0m);

        // Relationship: CabinClass belongs to Aircraft
        builder.HasOne(acc => acc.Aircraft)
            .WithMany(a => a.CabinClasses)
            .HasForeignKey(acc => acc.AircraftId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint: One cabin class per aircraft
        builder.HasIndex(acc => new { acc.AircraftId, acc.CabinClass })
            .IsUnique();
    }
}

