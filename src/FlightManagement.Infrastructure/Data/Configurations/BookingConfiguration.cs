using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Booking entity.
/// </summary>
public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(b => b.Id);

        // Booking reference (unique PNR)
        builder.Property(b => b.BookingReference)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(b => b.BookingReference)
            .IsUnique();

        // Status stored as string
        builder.Property(b => b.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(BookingStatus.Pending);

        builder.Property(b => b.TripType)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Pricing with precision
        builder.Property(b => b.BaseFare).HasPrecision(18, 2);
        builder.Property(b => b.TaxAmount).HasPrecision(18, 2);
        builder.Property(b => b.ServiceFee).HasPrecision(18, 2);
        builder.Property(b => b.SeatSelectionFees).HasPrecision(18, 2);
        builder.Property(b => b.ExtrasFees).HasPrecision(18, 2);
        builder.Property(b => b.DiscountAmount).HasPrecision(18, 2);
        builder.Property(b => b.TotalAmount).HasPrecision(18, 2);
        builder.Property(b => b.PaidAmount).HasPrecision(18, 2);
        builder.Property(b => b.RefundAmount).HasPrecision(18, 2);

        builder.Property(b => b.Currency)
            .IsRequired()
            .HasMaxLength(3);

        // Contact info
        builder.Property(b => b.ContactEmail)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(b => b.ContactPhone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(b => b.SpecialRequests)
            .HasMaxLength(1000);

        builder.Property(b => b.PromoCode)
            .HasMaxLength(50);

        // Payment status
        builder.Property(b => b.PaymentStatus)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Cancellation
        builder.Property(b => b.CancellationReason)
            .HasMaxLength(500);

        builder.Property(b => b.RefundStatus)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Relationship: Booking belongs to Customer
        builder.HasOne(b => b.Customer)
            .WithMany(c => c.Bookings)
            .HasForeignKey(b => b.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship: Booking has CancellationPolicy
        builder.HasOne(b => b.CancellationPolicy)
            .WithMany(cp => cp.Bookings)
            .HasForeignKey(b => b.CancellationPolicyId)
            .OnDelete(DeleteBehavior.SetNull);

        // Relationship: Booking may have Promotion
        builder.HasOne(b => b.Promotion)
            .WithMany()
            .HasForeignKey(b => b.PromotionId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(b => b.CustomerId);
        builder.HasIndex(b => b.Status);
        builder.HasIndex(b => b.BookingDate);
        builder.HasIndex(b => b.PaymentStatus);
        builder.HasIndex(b => new { b.CustomerId, b.Status });
        builder.HasIndex(b => b.PromotionId);
    }
}

