using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the PaymentRecord entity.
/// </summary>
public class PaymentRecordConfiguration : IEntityTypeConfiguration<PaymentRecord>
{
    public void Configure(EntityTypeBuilder<PaymentRecord> builder)
    {
        builder.ToTable("PaymentRecords");

        builder.HasKey(pr => pr.Id);

        // Transaction reference
        builder.Property(pr => pr.TransactionReference)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(pr => pr.TransactionReference)
            .IsUnique();

        // Payment type and method
        builder.Property(pr => pr.PaymentType)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(pr => pr.PaymentMethod)
            .HasConversion<string>()
            .HasMaxLength(20);

        // Amount
        builder.Property(pr => pr.Amount).HasPrecision(18, 2);

        builder.Property(pr => pr.Currency)
            .IsRequired()
            .HasMaxLength(3);

        // Status
        builder.Property(pr => pr.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(PaymentStatus.Pending);

        // Gateway info
        builder.Property(pr => pr.GatewayReference)
            .HasMaxLength(100);

        builder.Property(pr => pr.GatewayResponse)
            .HasMaxLength(4000);

        builder.Property(pr => pr.CardLastFour)
            .HasMaxLength(4);

        builder.Property(pr => pr.CardBrand)
            .HasMaxLength(20);

        builder.Property(pr => pr.FailureReason)
            .HasMaxLength(500);

        builder.Property(pr => pr.IpAddress)
            .HasMaxLength(45);

        // Relationship: PaymentRecord belongs to Booking
        builder.HasOne(pr => pr.Booking)
            .WithMany(b => b.PaymentRecords)
            .HasForeignKey(pr => pr.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes
        builder.HasIndex(pr => pr.BookingId);
        builder.HasIndex(pr => pr.Status);
        builder.HasIndex(pr => pr.ProcessedAt);
    }
}

