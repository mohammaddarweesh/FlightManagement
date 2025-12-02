using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

public class OverbookingPolicyConfiguration : IEntityTypeConfiguration<OverbookingPolicy>
{
    public void Configure(EntityTypeBuilder<OverbookingPolicy> builder)
    {
        builder.ToTable("OverbookingPolicies");

        builder.HasKey(op => op.Id);

        builder.Property(op => op.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(op => op.Description)
            .HasMaxLength(1000);

        builder.Property(op => op.MaxOverbookingPercentage)
            .HasPrecision(5, 2);

        // Relationships
        builder.HasOne(op => op.Airline)
            .WithMany(a => a.OverbookingPolicies)
            .HasForeignKey(op => op.AirlineId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(op => op.DepartureAirport)
            .WithMany()
            .HasForeignKey(op => op.DepartureAirportId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(op => op.ArrivalAirport)
            .WithMany()
            .HasForeignKey(op => op.ArrivalAirportId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(op => op.AirlineId);
        builder.HasIndex(op => op.IsActive);
        builder.HasIndex(op => op.Priority);
    }
}

