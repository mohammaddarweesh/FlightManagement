using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

public class SeasonalPricingConfiguration : IEntityTypeConfiguration<SeasonalPricing>
{
    public void Configure(EntityTypeBuilder<SeasonalPricing> builder)
    {
        builder.ToTable("SeasonalPricings");

        builder.HasKey(sp => sp.Id);

        builder.Property(sp => sp.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(sp => sp.Description)
            .HasMaxLength(1000);

        builder.Property(sp => sp.AdjustmentPercentage)
            .HasPrecision(10, 4);

        // Relationships
        builder.HasOne(sp => sp.Airline)
            .WithMany()
            .HasForeignKey(sp => sp.AirlineId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(sp => sp.DepartureAirport)
            .WithMany()
            .HasForeignKey(sp => sp.DepartureAirportId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(sp => sp.ArrivalAirport)
            .WithMany()
            .HasForeignKey(sp => sp.ArrivalAirportId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(sp => sp.SeasonType);
        builder.HasIndex(sp => sp.StartDate);
        builder.HasIndex(sp => sp.EndDate);
        builder.HasIndex(sp => sp.IsActive);
        builder.HasIndex(sp => new { sp.StartDate, sp.EndDate });
    }
}

