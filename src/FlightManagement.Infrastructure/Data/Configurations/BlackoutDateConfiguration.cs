using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

public class BlackoutDateConfiguration : IEntityTypeConfiguration<BlackoutDate>
{
    public void Configure(EntityTypeBuilder<BlackoutDate> builder)
    {
        builder.ToTable("BlackoutDates");

        builder.HasKey(bd => bd.Id);

        builder.Property(bd => bd.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(bd => bd.Description)
            .HasMaxLength(1000);

        // Relationships
        builder.HasOne(bd => bd.Airline)
            .WithMany()
            .HasForeignKey(bd => bd.AirlineId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(bd => bd.DepartureAirport)
            .WithMany()
            .HasForeignKey(bd => bd.DepartureAirportId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne(bd => bd.ArrivalAirport)
            .WithMany()
            .HasForeignKey(bd => bd.ArrivalAirportId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(bd => bd.StartDate);
        builder.HasIndex(bd => bd.EndDate);
        builder.HasIndex(bd => bd.IsActive);
        builder.HasIndex(bd => new { bd.StartDate, bd.EndDate });
    }
}

