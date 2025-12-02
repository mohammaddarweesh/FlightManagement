using FlightManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds cancellation policies with associated rules.
/// </summary>
public class CancellationPolicySeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CancellationPolicySeeder> _logger;

    public CancellationPolicySeeder(ApplicationDbContext context, ILogger<CancellationPolicySeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Set<CancellationPolicy>().AnyAsync())
        {
            _logger.LogInformation("Cancellation policies already seeded");
            return;
        }

        var flexiblePolicy = new CancellationPolicy
        {
            Id = Guid.NewGuid(),
            Code = "FLEX",
            Name = "Flexible",
            Description = "Full refund up to 24 hours before departure",
            IsRefundable = true,
            IsActive = true
        };
        flexiblePolicy.Rules.Add(new CancellationPolicyRule
        {
            Id = Guid.NewGuid(),
            CancellationPolicyId = flexiblePolicy.Id,
            MinHoursBeforeDeparture = 24,
            MaxHoursBeforeDeparture = null,
            RefundPercentage = 100,
            FlatFee = 0,
            Currency = "USD"
        });
        flexiblePolicy.Rules.Add(new CancellationPolicyRule
        {
            Id = Guid.NewGuid(),
            CancellationPolicyId = flexiblePolicy.Id,
            MinHoursBeforeDeparture = 0,
            MaxHoursBeforeDeparture = 24,
            RefundPercentage = 50,
            FlatFee = 25,
            Currency = "USD"
        });

        var standardPolicy = new CancellationPolicy
        {
            Id = Guid.NewGuid(),
            Code = "STANDARD",
            Name = "Standard",
            Description = "Partial refund with fees based on timing",
            IsRefundable = true,
            IsActive = true
        };
        standardPolicy.Rules.Add(new CancellationPolicyRule
        {
            Id = Guid.NewGuid(),
            CancellationPolicyId = standardPolicy.Id,
            MinHoursBeforeDeparture = 72,
            MaxHoursBeforeDeparture = null,
            RefundPercentage = 90,
            FlatFee = 50,
            Currency = "USD"
        });
        standardPolicy.Rules.Add(new CancellationPolicyRule
        {
            Id = Guid.NewGuid(),
            CancellationPolicyId = standardPolicy.Id,
            MinHoursBeforeDeparture = 24,
            MaxHoursBeforeDeparture = 72,
            RefundPercentage = 50,
            FlatFee = 75,
            Currency = "USD"
        });
        standardPolicy.Rules.Add(new CancellationPolicyRule
        {
            Id = Guid.NewGuid(),
            CancellationPolicyId = standardPolicy.Id,
            MinHoursBeforeDeparture = 0,
            MaxHoursBeforeDeparture = 24,
            RefundPercentage = 0,
            FlatFee = 0,
            Currency = "USD"
        });

        var nonRefundablePolicy = new CancellationPolicy
        {
            Id = Guid.NewGuid(),
            Code = "NONREF",
            Name = "Non-Refundable",
            Description = "No refunds available",
            IsRefundable = false,
            IsActive = true
        };

        var businessPolicy = new CancellationPolicy
        {
            Id = Guid.NewGuid(),
            Code = "BUSINESS",
            Name = "Business Flex",
            Description = "Premium cancellation policy for business travelers",
            IsRefundable = true,
            IsActive = true
        };
        businessPolicy.Rules.Add(new CancellationPolicyRule
        {
            Id = Guid.NewGuid(),
            CancellationPolicyId = businessPolicy.Id,
            MinHoursBeforeDeparture = 2,
            MaxHoursBeforeDeparture = null,
            RefundPercentage = 100,
            FlatFee = 0,
            Currency = "USD"
        });
        businessPolicy.Rules.Add(new CancellationPolicyRule
        {
            Id = Guid.NewGuid(),
            CancellationPolicyId = businessPolicy.Id,
            MinHoursBeforeDeparture = 0,
            MaxHoursBeforeDeparture = 2,
            RefundPercentage = 80,
            FlatFee = 0,
            Currency = "USD"
        });

        var policies = new List<CancellationPolicy> { flexiblePolicy, standardPolicy, nonRefundablePolicy, businessPolicy };

        await _context.Set<CancellationPolicy>().AddRangeAsync(policies);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} cancellation policies", policies.Count);
    }
}

