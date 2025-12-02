using FlightManagement.Application.Common.Interfaces;
using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds sample customers with user accounts.
/// </summary>
public class CustomerSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<CustomerSeeder> _logger;

    public CustomerSeeder(
        ApplicationDbContext context,
        IPasswordHasher passwordHasher,
        ILogger<CustomerSeeder> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Customers.AnyAsync())
        {
            _logger.LogInformation("Customers already seeded");
            return;
        }

        var customers = new List<(User User, Customer Customer)>
        {
            CreateCustomer("john.doe@example.com", "John", "Doe", "+1-555-0101", "United States"),
            CreateCustomer("jane.smith@example.com", "Jane", "Smith", "+1-555-0102", "United States"),
            CreateCustomer("robert.johnson@example.com", "Robert", "Johnson", "+1-555-0103", "United States"),
            CreateCustomer("emily.williams@example.com", "Emily", "Williams", "+44-20-7946-0958", "United Kingdom"),
            CreateCustomer("michael.brown@example.com", "Michael", "Brown", "+44-20-7946-0959", "United Kingdom"),
            CreateCustomer("sarah.davis@example.com", "Sarah", "Davis", "+33-1-42-68-53-00", "France"),
            CreateCustomer("david.miller@example.com", "David", "Miller", "+49-30-1234567", "Germany"),
            CreateCustomer("lisa.wilson@example.com", "Lisa", "Wilson", "+61-2-9374-4000", "Australia"),
            CreateCustomer("james.taylor@example.com", "James", "Taylor", "+1-416-555-0100", "Canada"),
            CreateCustomer("emma.anderson@example.com", "Emma", "Anderson", "+65-6123-4567", "Singapore"),
            CreateCustomer("thomas.white@example.com", "Thomas", "White", "+971-4-123-4567", "United Arab Emirates"),
            CreateCustomer("olivia.harris@example.com", "Olivia", "Harris", "+81-3-1234-5678", "Japan"),
            CreateCustomer("william.martin@example.com", "William", "Martin", "+31-20-123-4567", "Netherlands"),
            CreateCustomer("sophia.garcia@example.com", "Sophia", "Garcia", "+34-91-123-4567", "Spain"),
            CreateCustomer("daniel.martinez@example.com", "Daniel", "Martinez", "+52-55-1234-5678", "Mexico")
        };

        foreach (var (user, customer) in customers)
        {
            await _context.Users.AddAsync(user);
            customer.UserId = user.Id;
            await _context.Customers.AddAsync(customer);
        }

        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} customers", customers.Count);
    }

    private (User, Customer) CreateCustomer(string email, string firstName, string lastName,
        string phone, string country)
    {
        var userId = Guid.NewGuid();
        var customerId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            Email = email,
            PasswordHash = _passwordHasher.HashPassword("Customer123!"),
            UserType = UserType.Customer,
            IsEmailVerified = true,
            IsActive = true,
            CreatedAt = DateTime.UtcNow.AddDays(-new Random().Next(30, 365))
        };

        var customer = new Customer
        {
            Id = customerId,
            UserId = userId,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phone,
            DateOfBirth = DateTime.UtcNow.AddYears(-new Random().Next(25, 55)),
            Country = country,
            PreferredLanguage = "en",
            PreferredCurrency = country == "United Kingdom" ? "GBP" :
                               country == "France" || country == "Germany" ||
                               country == "Netherlands" || country == "Spain" ? "EUR" : "USD",
            ReceivePromotionalEmails = true,
            ReceiveSmsNotifications = false
        };

        return (user, customer);
    }
}

