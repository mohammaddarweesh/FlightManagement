using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds sample bookings with passengers and segments.
/// </summary>
public class BookingSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<BookingSeeder> _logger;

    public BookingSeeder(ApplicationDbContext context, ILogger<BookingSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Bookings.AnyAsync())
        {
            _logger.LogInformation("Bookings already seeded");
            return;
        }

        var customers = await _context.Customers.Include(c => c.User).ToListAsync();
        var flights = await _context.Flights
            .Include(f => f.FlightPricings)
            .Where(f => f.ScheduledDepartureTime > DateTime.UtcNow)
            .Take(50)
            .ToListAsync();
        var cancellationPolicies = await _context.Set<CancellationPolicy>().ToListAsync();

        if (!customers.Any() || !flights.Any())
        {
            _logger.LogWarning("Missing customers or flights. Skipping booking seeding.");
            return;
        }

        var random = new Random(42);
        var bookings = new List<Booking>();

        // Create bookings for each customer
        foreach (var customer in customers.Take(10))
        {
            var numBookings = random.Next(1, 4);

            for (int i = 0; i < numBookings && i < flights.Count; i++)
            {
                var flight = flights[random.Next(flights.Count)];
                var pricing = flight.FlightPricings.FirstOrDefault();
                if (pricing == null) continue;

                var booking = CreateBooking(customer, flight, pricing, cancellationPolicies, random);
                bookings.Add(booking);
            }
        }

        await _context.Bookings.AddRangeAsync(bookings);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} bookings", bookings.Count);
    }

    private Booking CreateBooking(Customer customer, Flight flight, FlightPricing pricing,
        List<CancellationPolicy> policies, Random random)
    {
        var bookingId = Guid.NewGuid();
        var numPassengers = random.Next(1, 4);
        var baseFare = pricing.BasePrice * numPassengers;
        var taxAmount = baseFare * 0.12m;
        var serviceFee = 25m;

        var booking = new Booking
        {
            Id = bookingId,
            BookingReference = GenerateBookingReference(random),
            CustomerId = customer.Id,
            Status = GetRandomStatus(random),
            BookingDate = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
            TripType = TripType.OneWay,
            BaseFare = baseFare,
            TaxAmount = taxAmount,
            ServiceFee = serviceFee,
            SeatSelectionFees = numPassengers * 15m,
            ExtrasFees = 0,
            DiscountAmount = 0,
            TotalAmount = baseFare + taxAmount + serviceFee + (numPassengers * 15m),
            Currency = "USD",
            ContactEmail = customer.User.Email,
            ContactPhone = customer.PhoneNumber ?? "+1-555-0000",
            PaymentStatus = PaymentStatus.Completed,
            PaidAmount = baseFare + taxAmount + serviceFee + (numPassengers * 15m),
            CancellationPolicyId = policies.FirstOrDefault()?.Id
        };

        // Add passengers
        for (int i = 0; i < numPassengers; i++)
        {
            booking.Passengers.Add(CreatePassenger(bookingId, i == 0 ? customer : null, random));
        }

        // Add booking segment
        booking.Segments.Add(new BookingSegment
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            FlightId = flight.Id,
            SegmentOrder = 1,
            CabinClass = pricing.CabinClass,
            Status = SegmentStatus.Confirmed,
            BaseFarePerPax = baseFare / numPassengers,
            TaxPerPax = taxAmount / numPassengers,
            SegmentSubtotal = baseFare + taxAmount,
            CheckedBaggageAllowanceKg = 23,
            CabinBaggageAllowanceKg = 7
        });

        // Add booking history
        booking.History.Add(new BookingHistory
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            Action = BookingAction.Created,
            PerformedByType = ActorType.Customer,
            PerformedBy = customer.Id,
            PerformedAt = booking.BookingDate,
            Description = "Booking created"
        });

        return booking;
    }

    private static Passenger CreatePassenger(Guid bookingId, Customer? customer, Random random)
    {
        var firstNames = new[] { "John", "Jane", "Michael", "Sarah", "David", "Emily", "Robert", "Lisa" };
        var lastNames = new[] { "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis" };

        return new Passenger
        {
            Id = Guid.NewGuid(),
            BookingId = bookingId,
            FirstName = customer?.FirstName ?? firstNames[random.Next(firstNames.Length)],
            LastName = customer?.LastName ?? lastNames[random.Next(lastNames.Length)],
            DateOfBirth = DateTime.UtcNow.AddYears(-random.Next(18, 60)),
            Gender = random.Next(2) == 0 ? Gender.Male : Gender.Female,
            PassengerType = PassengerType.Adult,
            Nationality = "US",
            PassportNumber = $"P{random.Next(10000000, 99999999)}",
            PassportIssuingCountry = "US",
            PassportExpiryDate = DateTime.UtcNow.AddYears(random.Next(2, 10)),
            Email = customer?.User?.Email ?? $"passenger{random.Next(1000)}@example.com",
            Phone = customer?.PhoneNumber ?? $"+1-555-{random.Next(1000, 9999)}"
        };
    }

    private static string GenerateBookingReference(Random random)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Range(0, 6).Select(_ => chars[random.Next(chars.Length)]).ToArray());
    }

    private static BookingStatus GetRandomStatus(Random random)
    {
        var statuses = new[] { BookingStatus.Confirmed, BookingStatus.Confirmed, BookingStatus.Confirmed,
            BookingStatus.Pending, BookingStatus.Completed };
        return statuses[random.Next(statuses.Length)];
    }
}

