using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightManagement.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the CancellationPolicyRule entity.
/// </summary>
public class CancellationPolicyRuleConfiguration : IEntityTypeConfiguration<CancellationPolicyRule>
{
    public void Configure(EntityTypeBuilder<CancellationPolicyRule> builder)
    {
        builder.ToTable("CancellationPolicyRules");

        builder.HasKey(cpr => cpr.Id);

        // Refund percentage
        builder.Property(cpr => cpr.RefundPercentage)
            .HasPrecision(5, 2);

        // Flat fee
        builder.Property(cpr => cpr.FlatFee)
            .HasPrecision(18, 2);

        builder.Property(cpr => cpr.Currency)
            .IsRequired()
            .HasMaxLength(3);

        // Relationship: CancellationPolicyRule belongs to CancellationPolicy
        builder.HasOne(cpr => cpr.CancellationPolicy)
            .WithMany(cp => cp.Rules)
            .HasForeignKey(cpr => cpr.CancellationPolicyId)
            .OnDelete(DeleteBehavior.Cascade);

        // Index for policy lookup
        builder.HasIndex(cpr => cpr.CancellationPolicyId);

        // Index for finding applicable rule by hours
        builder.HasIndex(cpr => new { cpr.CancellationPolicyId, cpr.MinHoursBeforeDeparture });
    }
}

