using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Aircraft entity.
/// Defines table structure, relationships, and constraints.
/// </summary>
public class AircraftConfiguration : IEntityTypeConfiguration<Aircraft>
{
    public void Configure(EntityTypeBuilder<Aircraft> builder)
    {
        builder.ToTable("Aircraft");

        builder.HasKey(a => a.Id);

        // Model name
        builder.Property(a => a.Model)
            .IsRequired()
            .HasMaxLength(100);

        // Manufacturer
        builder.Property(a => a.Manufacturer)
            .IsRequired()
            .HasMaxLength(100);

        // Registration number - unique identifier
        builder.Property(a => a.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(a => a.RegistrationNumber)
            .IsUnique();

        // Total seats
        builder.Property(a => a.TotalSeats)
            .IsRequired();

        // Index for active aircraft
        builder.HasIndex(a => a.IsActive);

        // Relationship: Aircraft belongs to Airline
        builder.HasOne(a => a.Airline)
            .WithMany(al => al.Aircraft)
            .HasForeignKey(a => a.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);

        // Index for airline lookup
        builder.HasIndex(a => a.AirlineId);
    }
}

