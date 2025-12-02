using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Amenity entity.
/// Master catalog of available amenities.
/// </summary>
public class AmenityConfiguration : IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.ToTable("Amenities");

        builder.HasKey(a => a.Id);

        // Unique code
        builder.Property(a => a.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(a => a.Code)
            .IsUnique();

        // Name
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Description
        builder.Property(a => a.Description)
            .IsRequired()
            .HasMaxLength(500);

        // Category stored as string
        builder.Property(a => a.Category)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Icon URL
        builder.Property(a => a.IconUrl)
            .HasMaxLength(500);

        // Index for category filtering
        builder.HasIndex(a => a.Category);

        // Index for active amenities
        builder.HasIndex(a => a.IsActive);
    }
}

