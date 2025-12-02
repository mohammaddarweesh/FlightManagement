using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeder for creating the default admin user
/// </summary>
public class AdminSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AdminSeeder> _logger;

    public AdminSeeder(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher,
        IConfiguration configuration,
        ILogger<AdminSeeder> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Seeds the default admin user if it doesn't exist
    /// </summary>
    public async Task SeedAsync()
    {
        var adminEmail = _configuration["AdminUser:Email"] ?? "admin@flightmanagement.com";
        var adminPassword = _configuration["AdminUser:Password"] ?? "Admin@123456";

        // Check if admin already exists
        var existingAdmin = await _context.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(u => u.Email == adminEmail.ToLower());

        if (existingAdmin != null)
        {
            _logger.LogInformation("Admin user already exists: {Email}", adminEmail);
            
            // Ensure existing user is admin
            if (existingAdmin.UserType != UserType.Admin)
            {
                existingAdmin.UserType = UserType.Admin;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Updated existing user to Admin: {Email}", adminEmail);
            }
            return;
        }

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Email = adminEmail.ToLower(),
            PasswordHash = _passwordHasher.HashPassword(adminPassword),
            UserType = UserType.Admin,
            IsEmailVerified = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _context.Users.AddAsync(admin);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Admin user created successfully: {Email}", adminEmail);
    }
}

