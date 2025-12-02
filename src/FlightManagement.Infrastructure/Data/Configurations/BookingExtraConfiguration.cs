using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the BookingExtra entity.
/// </summary>
public class BookingExtraConfiguration : IEntityTypeConfiguration<BookingExtra>
{
    public void Configure(EntityTypeBuilder<BookingExtra> builder)
    {
        builder.ToTable("BookingExtras");

        builder.HasKey(be => be.Id);

        // Extra type
        builder.Property(be => be.ExtraType)
            .HasConversion<string>()
            .HasMaxLength(30);

        // Description
        builder.Property(be => be.Description)
            .IsRequired()
            .HasMaxLength(500);

        // Pricing
        builder.Property(be => be.UnitPrice).HasPrecision(18, 2);
        builder.Property(be => be.TotalPrice).HasPrecision(18, 2);

        builder.Property(be => be.Currency)
            .IsRequired()
            .HasMaxLength(3);

        // Status
        builder.Property(be => be.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(ExtraStatus.Confirmed);

        // Relationship: BookingExtra belongs to Booking
        builder.HasOne(be => be.Booking)
            .WithMany(b => b.Extras)
            .HasForeignKey(be => be.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship: BookingExtra optionally belongs to BookingSegment
        builder.HasOne(be => be.BookingSegment)
            .WithMany()
            .HasForeignKey(be => be.BookingSegmentId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relationship: BookingExtra optionally belongs to Passenger
        builder.HasOne(be => be.Passenger)
            .WithMany()
            .HasForeignKey(be => be.PassengerId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relationship: BookingExtra optionally references FlightAmenity
        builder.HasOne(be => be.FlightAmenity)
            .WithMany()
            .HasForeignKey(be => be.FlightAmenityId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(be => be.BookingId);
        builder.HasIndex(be => be.ExtraType);
    }
}

