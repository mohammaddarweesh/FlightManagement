using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the PassengerSeat entity.
/// </summary>
public class PassengerSeatConfiguration : IEntityTypeConfiguration<PassengerSeat>
{
    public void Configure(EntityTypeBuilder<PassengerSeat> builder)
    {
        builder.ToTable("PassengerSeats");

        builder.HasKey(ps => ps.Id);

        // Seat number
        builder.Property(ps => ps.SeatNumber)
            .IsRequired()
            .HasMaxLength(5);

        // Seat fee
        builder.Property(ps => ps.SeatFee).HasPrecision(18, 2);

        // Assignment type
        builder.Property(ps => ps.AssignmentType)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Relationship: PassengerSeat belongs to Passenger
        builder.HasOne(ps => ps.Passenger)
            .WithMany(p => p.PassengerSeats)
            .HasForeignKey(ps => ps.PassengerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship: PassengerSeat belongs to BookingSegment
        builder.HasOne(ps => ps.BookingSegment)
            .WithMany(bs => bs.PassengerSeats)
            .HasForeignKey(ps => ps.BookingSegmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship: PassengerSeat references FlightSeat
        builder.HasOne(ps => ps.FlightSeat)
            .WithMany(fs => fs.PassengerSeats)
            .HasForeignKey(ps => ps.FlightSeatId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(ps => ps.PassengerId);
        builder.HasIndex(ps => ps.BookingSegmentId);
        builder.HasIndex(ps => ps.FlightSeatId);

        // Unique constraint: One passenger per segment
        builder.HasIndex(ps => new { ps.PassengerId, ps.BookingSegmentId })
            .IsUnique();
    }
}

