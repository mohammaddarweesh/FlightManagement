using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Flight entity.
/// Core entity with multiple relationships and indexes.
/// </summary>
public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
    public void Configure(EntityTypeBuilder<Flight> builder)
    {
        builder.ToTable("Flights");

        builder.HasKey(f => f.Id);

        // Flight number
        builder.Property(f => f.FlightNumber)
            .IsRequired()
            .HasMaxLength(10);

        // Terminals and gates (optional)
        builder.Property(f => f.DepartureTerminal).HasMaxLength(20);
        builder.Property(f => f.ArrivalTerminal).HasMaxLength(20);
        builder.Property(f => f.DepartureGate).HasMaxLength(10);
        builder.Property(f => f.ArrivalGate).HasMaxLength(10);

        // Flight status stored as string
        builder.Property(f => f.Status)
            .HasConversion<string>()
            .HasMaxLength(20)
            .HasDefaultValue(FlightStatus.Scheduled);

        // Duration stored as ticks (TimeSpan)
        builder.Property(f => f.Duration)
            .IsRequired();

        // Relationship: Flight belongs to Airline
        builder.HasOne(f => f.Airline)
            .WithMany(a => a.Flights)
            .HasForeignKey(f => f.AirlineId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship: Flight uses Aircraft
        builder.HasOne(f => f.Aircraft)
            .WithMany(a => a.Flights)
            .HasForeignKey(f => f.AircraftId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship: Flight departs from Airport
        builder.HasOne(f => f.DepartureAirport)
            .WithMany(a => a.DepartureFlights)
            .HasForeignKey(f => f.DepartureAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship: Flight arrives at Airport
        builder.HasOne(f => f.ArrivalAirport)
            .WithMany(a => a.ArrivalFlights)
            .HasForeignKey(f => f.ArrivalAirportId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for flight search
        builder.HasIndex(f => f.FlightNumber);
        builder.HasIndex(f => f.ScheduledDepartureTime);
        builder.HasIndex(f => f.Status);
        builder.HasIndex(f => f.IsActive);

        // Composite index for route search
        builder.HasIndex(f => new { f.DepartureAirportId, f.ArrivalAirportId, f.ScheduledDepartureTime });

        // Composite index for airline flights
        builder.HasIndex(f => new { f.AirlineId, f.ScheduledDepartureTime });
    }
}

