using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

public class PromotionUsageConfiguration : IEntityTypeConfiguration<PromotionUsage>
{
    public void Configure(EntityTypeBuilder<PromotionUsage> builder)
    {
        builder.ToTable("PromotionUsages");

        builder.HasKey(pu => pu.Id);

        builder.Property(pu => pu.DiscountAmount)
            .HasPrecision(18, 2);

        builder.Property(pu => pu.Currency)
            .IsRequired()
            .HasMaxLength(3);

        // Relationships
        builder.HasOne(pu => pu.Promotion)
            .WithMany(p => p.Usages)
            .HasForeignKey(pu => pu.PromotionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pu => pu.Customer)
            .WithMany(c => c.PromotionUsages)
            .HasForeignKey(pu => pu.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pu => pu.Booking)
            .WithMany(b => b.PromotionUsages)
            .HasForeignKey(pu => pu.BookingId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes
        builder.HasIndex(pu => pu.PromotionId);
        builder.HasIndex(pu => pu.CustomerId);
        builder.HasIndex(pu => pu.BookingId);
        builder.HasIndex(pu => new { pu.PromotionId, pu.CustomerId });
    }
}

