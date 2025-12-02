using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(c => c.AddressLine1)
            .HasMaxLength(200);

        builder.Property(c => c.AddressLine2)
            .HasMaxLength(200);

        builder.Property(c => c.City)
            .HasMaxLength(100);

        builder.Property(c => c.State)
            .HasMaxLength(100);

        builder.Property(c => c.PostalCode)
            .HasMaxLength(20);

        builder.Property(c => c.Country)
            .HasMaxLength(100);

        builder.Property(c => c.PreferredLanguage)
            .HasMaxLength(10);

        builder.Property(c => c.PreferredCurrency)
            .HasMaxLength(10);

        // Ignore computed property
        builder.Ignore(c => c.FullName);

        // Soft delete filter
        builder.HasQueryFilter(c => !c.IsDeleted);

        // Index for UserId
        builder.HasIndex(c => c.UserId)
            .IsUnique();
    }
}

