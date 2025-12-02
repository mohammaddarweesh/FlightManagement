using FlightManagement.API.Middleware;
using FlightManagement.API.Services;
using FlightManagement.Application;
using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Infrastructure;
using FlightManagement.Infrastructure.BackgroundJobs;
using FlightManagement.Infrastructure.Data;
using FlightManagement.Infrastructure.Data.Seeders;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog with PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .Enrich.WithMachineName()
    .Enrich.WithEnvironmentName()
    .WriteTo.Console()
    .WriteTo.PostgreSQL(
        connectionString: connectionString,
        tableName: "Logs",
        needAutoCreateTable: true,
        columnOptions: new Dictionary<string, ColumnWriterBase>
        {
            { "Message", new RenderedMessageColumnWriter(NpgsqlDbType.Text) },
            { "MessageTemplate", new MessageTemplateColumnWriter(NpgsqlDbType.Text) },
            { "Level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
            { "TimeStamp", new TimestampColumnWriter(NpgsqlDbType.TimestampTz) },
            { "Exception", new ExceptionColumnWriter(NpgsqlDbType.Text) },
            { "LogEvent", new LogEventSerializedColumnWriter(NpgsqlDbType.Jsonb) },
            { "Properties", new PropertiesColumnWriter(NpgsqlDbType.Jsonb) }
        })
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Flight Management API",
        Version = "v1",
        Description = "API for Flight Management System"
    });

    // Add JWT Authentication support
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token. Example: eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Add HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register application services
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

// Add layer dependencies
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddHangfireServices(builder.Configuration);

// Add authorization policies
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"))
    .AddPolicy("RequireCustomer", policy => policy.RequireRole("Customer", "Admin"))
    .AddPolicy("RequireAdminOrStaff", policy => policy.RequireRole("Admin", "Staff"));

var app = builder.Build();

// Run migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        Log.Information("Running database migrations...");
        context.Database.Migrate();
        Log.Information("Database migrations completed successfully");

        // Seed all data using DatabaseSeeder
        var databaseSeeder = services.GetRequiredService<DatabaseSeeder>();
        await databaseSeeder.SeedAllAsync();
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while migrating/seeding the database");
        throw;
    }
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use custom middleware
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configure Hangfire
app.UseHangfireConfiguration();

try
{
    Log.Information("Starting FlightManagement API");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
