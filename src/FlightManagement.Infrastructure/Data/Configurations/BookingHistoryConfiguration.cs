using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the BookingHistory entity.
/// </summary>
public class BookingHistoryConfiguration : IEntityTypeConfiguration<BookingHistory>
{
    public void Configure(EntityTypeBuilder<BookingHistory> builder)
    {
        builder.ToTable("BookingHistory");

        builder.HasKey(bh => bh.Id);

        // Action
        builder.Property(bh => bh.Action)
            .HasConversion<string>()
            .HasMaxLength(30);

        // Description
        builder.Property(bh => bh.Description)
            .IsRequired()
            .HasMaxLength(500);

        // Old/New values (JSON)
        builder.Property(bh => bh.OldValues)
            .HasMaxLength(4000);

        builder.Property(bh => bh.NewValues)
            .HasMaxLength(4000);

        // Actor type
        builder.Property(bh => bh.PerformedByType)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Client info
        builder.Property(bh => bh.IpAddress)
            .HasMaxLength(45);

        builder.Property(bh => bh.UserAgent)
            .HasMaxLength(500);

        // Relationship: BookingHistory belongs to Booking
        builder.HasOne(bh => bh.Booking)
            .WithMany(b => b.History)
            .HasForeignKey(bh => bh.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(bh => bh.BookingId);
        builder.HasIndex(bh => bh.PerformedAt);
        builder.HasIndex(bh => bh.Action);
        builder.HasIndex(bh => new { bh.BookingId, bh.PerformedAt });
    }
}

