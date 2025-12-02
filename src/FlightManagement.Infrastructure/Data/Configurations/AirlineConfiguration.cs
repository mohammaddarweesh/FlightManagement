using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Airline entity.
/// Defines table structure, indexes, and constraints.
/// </summary>
public class AirlineConfiguration : IEntityTypeConfiguration<Airline>
{
    public void Configure(EntityTypeBuilder<Airline> builder)
    {
        builder.ToTable("Airlines");

        builder.HasKey(a => a.Id);

        // IATA code - unique, required, 2 chars
        builder.Property(a => a.IataCode)
            .IsRequired()
            .HasMaxLength(2);

        builder.HasIndex(a => a.IataCode)
            .IsUnique();

        // ICAO code - unique, required, 3 chars
        builder.Property(a => a.IcaoCode)
            .IsRequired()
            .HasMaxLength(3);

        builder.HasIndex(a => a.IcaoCode)
            .IsUnique();

        // Airline name
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Country
        builder.Property(a => a.Country)
            .IsRequired()
            .HasMaxLength(100);

        // Logo URL (optional)
        builder.Property(a => a.LogoUrl)
            .HasMaxLength(500);

        // Index for active airlines
        builder.HasIndex(a => a.IsActive);
    }
}

