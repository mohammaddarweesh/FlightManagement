using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the BookingSegment entity.
/// </summary>
public class BookingSegmentConfiguration : IEntityTypeConfiguration<BookingSegment>
{
    public void Configure(EntityTypeBuilder<BookingSegment> builder)
    {
        builder.ToTable("BookingSegments");

        builder.HasKey(bs => bs.Id);

        // Cabin class
        builder.Property(bs => bs.CabinClass)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Pricing
        builder.Property(bs => bs.BaseFarePerPax).HasPrecision(18, 2);
        builder.Property(bs => bs.TaxPerPax).HasPrecision(18, 2);
        builder.Property(bs => bs.SegmentSubtotal).HasPrecision(18, 2);

        // Status
        builder.Property(bs => bs.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(SegmentStatus.Confirmed);

        // Relationship: BookingSegment belongs to Booking
        builder.HasOne(bs => bs.Booking)
            .WithMany(b => b.Segments)
            .HasForeignKey(bs => bs.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship: BookingSegment references Flight
        builder.HasOne(bs => bs.Flight)
            .WithMany(f => f.BookingSegments)
            .HasForeignKey(bs => bs.FlightId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(bs => bs.BookingId);
        builder.HasIndex(bs => bs.FlightId);
        builder.HasIndex(bs => new { bs.BookingId, bs.SegmentOrder });
    }
}

