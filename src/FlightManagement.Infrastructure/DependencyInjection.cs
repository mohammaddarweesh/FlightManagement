using System.Text;
using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Domain.Repositories;
using FlightManagement.Infrastructure.Data;
using FlightManagement.Infrastructure.Data.Seeders;
using FlightManagement.Infrastructure.Repositories;
using FlightManagement.Infrastructure.Services;
using FlightManagement.Notifications;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FlightManagement.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRedisCache(configuration);
        services.AddRepositories();
        services.AddBusinessServices();
        services.AddNotifications(configuration);
        services.AddAuthServices(configuration);
        services.AddSeeders();

        return services;
    }

    private static IServiceCollection AddSeeders(this IServiceCollection services)
    {
        // Core reference data seeders
        services.AddScoped<AdminSeeder>();
        services.AddScoped<AirportSeeder>();
        services.AddScoped<AirlineSeeder>();
        services.AddScoped<AmenitySeeder>();
        services.AddScoped<AircraftSeeder>();

        // Policy and business rule seeders
        services.AddScoped<CancellationPolicySeeder>();
        services.AddScoped<BookingPolicySeeder>();
        services.AddScoped<OverbookingPolicySeeder>();
        services.AddScoped<SeasonalPricingSeeder>();
        services.AddScoped<DynamicPricingRuleSeeder>();
        services.AddScoped<BlackoutDateSeeder>();
        services.AddScoped<PromotionSeeder>();

        // Operational data seeders
        services.AddScoped<FlightSeeder>();
        services.AddScoped<CustomerSeeder>();
        services.AddScoped<BookingSeeder>();

        // Master seeder
        services.AddScoped<DatabaseSeeder>();

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString, 
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }

    private static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnection = configuration.GetConnectionString("Redis");
        
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisConnection;
        });

        services.AddSingleton<StackExchange.Redis.IConnectionMultiplexer>(sp =>
            StackExchange.Redis.ConnectionMultiplexer.Connect(redisConnection!));

        services.AddSingleton<ICacheService, RedisCacheService>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        return services;
    }

    private static IServiceCollection AddBusinessServices(this IServiceCollection services)
    {
        services.AddScoped<IPricingService, PricingService>();
        services.AddScoped<IAvailabilityService, AvailabilityService>();
        services.AddScoped<IPromotionService, PromotionService>();

        return services;
    }

    private static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register auth services
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IEmailService, EmailService>();

        // Configure JWT authentication
        var jwtSecret = configuration["Jwt:Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");
        var key = Encoding.UTF8.GetBytes(jwtSecret);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization();

        return services;
    }
}

