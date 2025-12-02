using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the CancellationPolicy entity.
/// </summary>
public class CancellationPolicyConfiguration : IEntityTypeConfiguration<CancellationPolicy>
{
    public void Configure(EntityTypeBuilder<CancellationPolicy> builder)
    {
        builder.ToTable("CancellationPolicies");

        builder.HasKey(cp => cp.Id);

        // Code
        builder.Property(cp => cp.Code)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(cp => cp.Code)
            .IsUnique();

        // Name
        builder.Property(cp => cp.Name)
            .IsRequired()
            .HasMaxLength(100);

        // Description
        builder.Property(cp => cp.Description)
            .IsRequired()
            .HasMaxLength(1000);

        // Indexes
        builder.HasIndex(cp => cp.IsActive);
    }
}

