using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Airport entity.
/// Defines table structure, indexes, and constraints.
/// </summary>
public class AirportConfiguration : IEntityTypeConfiguration<Airport>
{
    public void Configure(EntityTypeBuilder<Airport> builder)
    {
        builder.ToTable("Airports");

        builder.HasKey(a => a.Id);

        // IATA code - unique, required, 3 chars
        builder.Property(a => a.IataCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.HasIndex(a => a.IataCode)
            .IsUnique();

        // ICAO code - unique, required, 4 chars
        builder.Property(a => a.IcaoCode)
            .IsRequired()
            .HasMaxLength(4);

        builder.HasIndex(a => a.IcaoCode)
            .IsUnique();

        // Airport name
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Location fields
        builder.Property(a => a.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.CountryCode)
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(a => a.Timezone)
            .IsRequired()
            .HasMaxLength(50);

        // Geographic coordinates with precision
        builder.Property(a => a.Latitude)
            .HasPrecision(10, 7);

        builder.Property(a => a.Longitude)
            .HasPrecision(10, 7);

        // Index for active airports (commonly queried)
        builder.HasIndex(a => a.IsActive);

        // Index for city searches
        builder.HasIndex(a => a.City);

        // Index for country searches
        builder.HasIndex(a => a.Country);
    }
}

