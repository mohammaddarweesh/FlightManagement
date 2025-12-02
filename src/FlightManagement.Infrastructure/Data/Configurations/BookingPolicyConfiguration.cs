using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

public class BookingPolicyConfiguration : IEntityTypeConfiguration<BookingPolicy>
{
    public void Configure(EntityTypeBuilder<BookingPolicy> builder)
    {
        builder.ToTable("BookingPolicies");

        builder.HasKey(bp => bp.Id);

        builder.Property(bp => bp.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(bp => bp.Code)
            .IsUnique();

        builder.Property(bp => bp.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(bp => bp.Description)
            .HasMaxLength(1000);

        builder.Property(bp => bp.ErrorMessage)
            .IsRequired()
            .HasMaxLength(500);

        // Relationships
        builder.HasOne(bp => bp.Airline)
            .WithMany()
            .HasForeignKey(bp => bp.AirlineId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(bp => bp.DepartureAirport)
            .WithMany()
            .HasForeignKey(bp => bp.DepartureAirportId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(bp => bp.ArrivalAirport)
            .WithMany()
            .HasForeignKey(bp => bp.ArrivalAirportId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(bp => bp.Type);
        builder.HasIndex(bp => bp.IsActive);
        builder.HasIndex(bp => bp.Priority);
    }
}

