using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

public class PromotionConfiguration : IEntityTypeConfiguration<Promotion>
{
    public void Configure(EntityTypeBuilder<Promotion> builder)
    {
        builder.ToTable("Promotions");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(p => p.Code)
            .IsUnique();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(1000);

        builder.Property(p => p.DiscountValue)
            .HasPrecision(18, 2);

        builder.Property(p => p.MaxDiscountAmount)
            .HasPrecision(18, 2);

        builder.Property(p => p.MinBookingAmount)
            .HasPrecision(18, 2);

        builder.Property(p => p.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(p => p.ApplicableRoutes)
            .HasMaxLength(4000);

        builder.Property(p => p.ApplicableCabinClasses)
            .HasMaxLength(200);

        builder.Property(p => p.ApplicableAirlineIds)
            .HasMaxLength(2000);

        // Indexes
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.ValidFrom);
        builder.HasIndex(p => p.ValidTo);
        builder.HasIndex(p => p.Type);
        builder.HasIndex(p => p.IsActive);
    }
}

