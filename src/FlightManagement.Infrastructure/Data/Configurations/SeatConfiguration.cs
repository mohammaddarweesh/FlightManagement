using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Seat entity.
/// Defines individual seat properties on aircraft.
/// </summary>
public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seats");

        builder.HasKey(s => s.Id);

        // Seat number (e.g., "12A")
        builder.Property(s => s.SeatNumber)
            .IsRequired()
            .HasMaxLength(5);

        // Column as single character
        builder.Property(s => s.Column)
            .IsRequired()
            .HasMaxLength(1);

        // Cabin class stored as string
        builder.Property(s => s.CabinClass)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        // Seat type stored as string
        builder.Property(s => s.SeatType)
            .HasConversion<string>()
            .HasMaxLength(10)
            .IsRequired();

        // Relationship: Seat belongs to Aircraft
        builder.HasOne(s => s.Aircraft)
            .WithMany(a => a.Seats)
            .HasForeignKey(s => s.AircraftId)
            .OnDelete(DeleteBehavior.Cascade);

        // Unique constraint: Seat number per aircraft
        builder.HasIndex(s => new { s.AircraftId, s.SeatNumber })
            .IsUnique();

        // Index for class filtering
        builder.HasIndex(s => new { s.AircraftId, s.CabinClass });

        // Index for active seats
        builder.HasIndex(s => s.IsActive);
    }
}

