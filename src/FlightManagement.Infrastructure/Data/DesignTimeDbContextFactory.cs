using FlightManagement.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace FlightManagement.Infrastructure.Data;

/// <summary>
/// Factory for creating DbContext at design time (for migrations)
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        optionsBuilder.UseNpgsql(connectionString);

        return new ApplicationDbContext(optionsBuilder.Options, new DesignTimeCurrentUserService());
    }
}

/// <summary>
/// Dummy implementation for design-time operations
/// </summary>
public class DesignTimeCurrentUserService : ICurrentUserService
{
    public string? UserId => "system";
    public string? UserName => "system";
    public bool IsAuthenticated => false;
}

