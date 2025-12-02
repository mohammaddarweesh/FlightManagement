using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

public class DynamicPricingRuleConfiguration : IEntityTypeConfiguration<DynamicPricingRule>
{
    public void Configure(EntityTypeBuilder<DynamicPricingRule> builder)
    {
        builder.ToTable("DynamicPricingRules");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.Description)
            .HasMaxLength(1000);

        builder.Property(r => r.AdjustmentPercentage)
            .HasPrecision(10, 4);

        builder.Property(r => r.FixedAdjustment)
            .HasPrecision(18, 2);

        builder.Property(r => r.Currency)
            .IsRequired()
            .HasMaxLength(3);

        builder.Property(r => r.MinBookingPercentage)
            .HasPrecision(5, 2);

        builder.Property(r => r.MaxBookingPercentage)
            .HasPrecision(5, 2);

        // Relationships
        builder.HasOne(r => r.Airline)
            .WithMany()
            .HasForeignKey(r => r.AirlineId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(r => r.DepartureAirport)
            .WithMany()
            .HasForeignKey(r => r.DepartureAirportId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(r => r.ArrivalAirport)
            .WithMany()
            .HasForeignKey(r => r.ArrivalAirportId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(r => r.RuleType);
        builder.HasIndex(r => r.IsActive);
        builder.HasIndex(r => r.Priority);
    }
}

