using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the FlightSeat entity.
/// Tracks seat availability for specific flights.
/// </summary>
public class FlightSeatConfiguration : IEntityTypeConfiguration<FlightSeat>
{
    public void Configure(EntityTypeBuilder<FlightSeat> builder)
    {
        builder.ToTable("FlightSeats");

        builder.HasKey(fs => fs.Id);

        // Status stored as string
        builder.Property(fs => fs.Status)
            .HasConversion<string>()
            .HasMaxLength(15)
            .HasDefaultValue(SeatStatus.Available);

        // Seat-specific price (optional)
        builder.Property(fs => fs.Price)
            .HasPrecision(10, 2);

        // Relationship: FlightSeat belongs to Flight
        builder.HasOne(fs => fs.Flight)
            .WithMany(f => f.FlightSeats)
            .HasForeignKey(fs => fs.FlightId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship: FlightSeat references Seat
        builder.HasOne(fs => fs.Seat)
            .WithMany(s => s.FlightSeats)
            .HasForeignKey(fs => fs.SeatId)
            .OnDelete(DeleteBehavior.Restrict);

        // Unique constraint: One record per seat per flight
        builder.HasIndex(fs => new { fs.FlightId, fs.SeatId })
            .IsUnique();

        // Index for availability queries
        builder.HasIndex(fs => new { fs.FlightId, fs.Status });

        // Index for locked seats cleanup
        builder.HasIndex(fs => fs.LockedUntil)
            .HasFilter("\"LockedUntil\" IS NOT NULL");

        // Index for booking reference
        builder.HasIndex(fs => fs.BookingId)
            .HasFilter("\"BookingId\" IS NOT NULL");

        // Ignore calculated properties
        builder.Ignore(fs => fs.IsLockExpired);
        builder.Ignore(fs => fs.CanBeReserved);
    }
}

