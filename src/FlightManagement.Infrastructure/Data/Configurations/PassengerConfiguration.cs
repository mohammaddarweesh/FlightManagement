using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the Passenger entity.
/// </summary>
public class PassengerConfiguration : IEntityTypeConfiguration<Passenger>
{
    public void Configure(EntityTypeBuilder<Passenger> builder)
    {
        builder.ToTable("Passengers");

        builder.HasKey(p => p.Id);

        // Passenger type
        builder.Property(p => p.PassengerType)
            .HasConversion<string>()
            .HasMaxLength(10);

        // Identity
        builder.Property(p => p.Title)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.MiddleName)
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Gender)
            .HasConversion<string>()
            .HasMaxLength(10);

        builder.Property(p => p.Nationality)
            .IsRequired()
            .HasMaxLength(2);

        // Travel documents
        builder.Property(p => p.PassportNumber)
            .HasMaxLength(20);

        builder.Property(p => p.PassportIssuingCountry)
            .HasMaxLength(2);

        // Contact
        builder.Property(p => p.Email)
            .HasMaxLength(255);

        builder.Property(p => p.Phone)
            .HasMaxLength(20);

        // Preferences
        builder.Property(p => p.MealPreference)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.Property(p => p.SpecialAssistance)
            .HasMaxLength(500);

        builder.Property(p => p.FrequentFlyerNumber)
            .HasMaxLength(20);

        // Relationship: Passenger belongs to Booking
        builder.HasOne(p => p.Booking)
            .WithMany(b => b.Passengers)
            .HasForeignKey(p => p.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        // Relationship: Passenger has FrequentFlyerAirline
        builder.HasOne(p => p.FrequentFlyerAirline)
            .WithMany(a => a.FrequentFlyers)
            .HasForeignKey(p => p.FrequentFlyerAirlineId)
            .OnDelete(DeleteBehavior.SetNull);

        // Indexes
        builder.HasIndex(p => p.BookingId);
        builder.HasIndex(p => p.PassportNumber);
        builder.HasIndex(p => new { p.BookingId, p.IsLeadPassenger });
    }
}

