using FlightManagement.Domain.Entities;
using FlightManagement.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.Data.Seeders;

/// <summary>
/// Seeds aircraft with their cabin configurations and seats.
/// </summary>
public class AircraftSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AircraftSeeder> _logger;

    public AircraftSeeder(ApplicationDbContext context, ILogger<AircraftSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        if (await _context.Aircraft.AnyAsync())
        {
            _logger.LogInformation("Aircraft already seeded");
            return;
        }

        // Get airlines to assign aircraft
        var airlines = await _context.Airlines.ToListAsync();
        if (!airlines.Any())
        {
            _logger.LogWarning("No airlines found. Run AirlineSeeder first.");
            return;
        }

        var americanAirlines = airlines.FirstOrDefault(a => a.IataCode == "AA");
        var emirates = airlines.FirstOrDefault(a => a.IataCode == "EK");
        var britishAirways = airlines.FirstOrDefault(a => a.IataCode == "BA");

        if (americanAirlines == null || emirates == null || britishAirways == null)
        {
            _logger.LogWarning("Required airlines not found");
            return;
        }

        // Create aircraft with cabin configurations
        var aircraft = new List<Aircraft>();

        // American Airlines Boeing 737-800
        var aa737 = CreateBoeing737(americanAirlines.Id, "N123AA");
        aircraft.Add(aa737);

        // American Airlines Boeing 777-300ER
        var aa777 = CreateBoeing777(americanAirlines.Id, "N456AA");
        aircraft.Add(aa777);

        // Emirates Airbus A380
        var ek380 = CreateAirbusA380(emirates.Id, "A6-EDA");
        aircraft.Add(ek380);

        // British Airways Boeing 787-9
        var ba787 = CreateBoeing787(britishAirways.Id, "G-ZBKA");
        aircraft.Add(ba787);

        await _context.Aircraft.AddRangeAsync(aircraft);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Seeded {Count} aircraft with cabin configurations and seats", aircraft.Count);
    }

    private Aircraft CreateBoeing737(Guid airlineId, string registration)
    {
        var aircraft = new Aircraft
        {
            Id = Guid.NewGuid(),
            AirlineId = airlineId,
            Model = "Boeing 737-800",
            Manufacturer = "Boeing",
            RegistrationNumber = registration,
            TotalSeats = 160,
            ManufactureYear = 2018,
            RangeKm = 5436,
            CruisingSpeedKmh = 842
        };

        // Cabin configurations
        aircraft.CabinClasses = new List<AircraftCabinClass>
        {
            new() { Id = Guid.NewGuid(), AircraftId = aircraft.Id, CabinClass = FlightClass.Business, SeatCount = 16, RowStart = 1, RowEnd = 4, SeatsPerRow = 4, SeatLayout = "2-2", BasePriceMultiplier = 3.0m },
            new() { Id = Guid.NewGuid(), AircraftId = aircraft.Id, CabinClass = FlightClass.Economy, SeatCount = 144, RowStart = 5, RowEnd = 29, SeatsPerRow = 6, SeatLayout = "3-3", BasePriceMultiplier = 1.0m }
        };

        // Generate seats
        aircraft.Seats = GenerateSeats(aircraft.Id, aircraft.CabinClasses.ToList());

        return aircraft;
    }

    private Aircraft CreateBoeing777(Guid airlineId, string registration)
    {
        var aircraft = new Aircraft
        {
            Id = Guid.NewGuid(),
            AirlineId = airlineId,
            Model = "Boeing 777-300ER",
            Manufacturer = "Boeing",
            RegistrationNumber = registration,
            TotalSeats = 304,
            ManufactureYear = 2020,
            RangeKm = 13650,
            CruisingSpeedKmh = 905
        };

        aircraft.CabinClasses = new List<AircraftCabinClass>
        {
            new() { Id = Guid.NewGuid(), AircraftId = aircraft.Id, CabinClass = FlightClass.First, SeatCount = 8, RowStart = 1, RowEnd = 2, SeatsPerRow = 4, SeatLayout = "1-2-1", BasePriceMultiplier = 6.0m },
            new() { Id = Guid.NewGuid(), AircraftId = aircraft.Id, CabinClass = FlightClass.Business, SeatCount = 52, RowStart = 3, RowEnd = 15, SeatsPerRow = 4, SeatLayout = "1-2-1", BasePriceMultiplier = 3.5m },
            new() { Id = Guid.NewGuid(), AircraftId = aircraft.Id, CabinClass = FlightClass.PremiumEconomy, SeatCount = 24, RowStart = 16, RowEnd = 19, SeatsPerRow = 6, SeatLayout = "2-2-2", BasePriceMultiplier = 1.8m },
            new() { Id = Guid.NewGuid(), AircraftId = aircraft.Id, CabinClass = FlightClass.Economy, SeatCount = 220, RowStart = 20, RowEnd = 42, SeatsPerRow = 10, SeatLayout = "3-4-3", BasePriceMultiplier = 1.0m }
        };

        aircraft.Seats = GenerateSeats(aircraft.Id, aircraft.CabinClasses.ToList());

        return aircraft;
    }

    private Aircraft CreateAirbusA380(Guid airlineId, string registration)
    {
        var aircraft = new Aircraft
        {
            Id = Guid.NewGuid(),
            AirlineId = airlineId,
            Model = "Airbus A380-800",
            Manufacturer = "Airbus",
            RegistrationNumber = registration,
            TotalSeats = 489,
            ManufactureYear = 2019,
            RangeKm = 15200,
            CruisingSpeedKmh = 903
        };

        aircraft.CabinClasses = new List<AircraftCabinClass>
        {
            new() { Id = Guid.NewGuid(), AircraftId = aircraft.Id, CabinClass = FlightClass.First, SeatCount = 14, RowStart = 1, RowEnd = 4, SeatsPerRow = 4, SeatLayout = "1-2-1", BasePriceMultiplier = 8.0m },
            new() { Id = Guid.NewGuid(), AircraftId = aircraft.Id, CabinClass = FlightClass.Business, SeatCount = 76, RowStart = 5, RowEnd = 24, SeatsPerRow = 4, SeatLayout = "1-2-1", BasePriceMultiplier = 4.0m },
            new() { Id = Guid.NewGuid(), AircraftId = aircraft.Id, CabinClass = FlightClass.Economy, SeatCount = 399, RowStart = 25, RowEnd = 65, SeatsPerRow = 10, SeatLayout = "3-4-3", BasePriceMultiplier = 1.0m }
        };

        aircraft.Seats = GenerateSeats(aircraft.Id, aircraft.CabinClasses.ToList());

        return aircraft;
    }

    private Aircraft CreateBoeing787(Guid airlineId, string registration)
    {
        var aircraft = new Aircraft
        {
            Id = Guid.NewGuid(),
            AirlineId = airlineId,
            Model = "Boeing 787-9 Dreamliner",
            Manufacturer = "Boeing",
            RegistrationNumber = registration,
            TotalSeats = 216,
            ManufactureYear = 2021,
            RangeKm = 14140,
            CruisingSpeedKmh = 903
        };

        aircraft.CabinClasses = new List<AircraftCabinClass>
        {
            new() { Id = Guid.NewGuid(), AircraftId = aircraft.Id, CabinClass = FlightClass.Business, SeatCount = 48, RowStart = 1, RowEnd = 12, SeatsPerRow = 4, SeatLayout = "1-2-1", BasePriceMultiplier = 4.5m },
            new() { Id = Guid.NewGuid(), AircraftId = aircraft.Id, CabinClass = FlightClass.PremiumEconomy, SeatCount = 21, RowStart = 13, RowEnd = 16, SeatsPerRow = 6, SeatLayout = "2-3-2", BasePriceMultiplier = 2.0m },
            new() { Id = Guid.NewGuid(), AircraftId = aircraft.Id, CabinClass = FlightClass.Economy, SeatCount = 147, RowStart = 17, RowEnd = 42, SeatsPerRow = 9, SeatLayout = "3-3-3", BasePriceMultiplier = 1.0m }
        };

        aircraft.Seats = GenerateSeats(aircraft.Id, aircraft.CabinClasses.ToList());

        return aircraft;
    }
    /// <summary>
    /// Generates individual seats based on cabin class configurations.
    /// </summary>
    private List<Seat> GenerateSeats(Guid aircraftId, List<AircraftCabinClass> cabinClasses)
    {
        var seats = new List<Seat>();

        foreach (var cabin in cabinClasses)
        {
            var columns = GetColumnsFromLayout(cabin.SeatLayout);

            for (var row = cabin.RowStart; row <= cabin.RowEnd; row++)
            {
                for (var i = 0; i < columns.Length; i++)
                {
                    var column = columns[i];
                    var seatType = DetermineSeatType(i, columns.Length, cabin.SeatLayout);
                    var isEmergencyExit = IsEmergencyExitRow(row, cabin.CabinClass);

                    seats.Add(new Seat
                    {
                        Id = Guid.NewGuid(),
                        AircraftId = aircraftId,
                        SeatNumber = $"{row}{column}",
                        Row = row,
                        Column = column,
                        CabinClass = cabin.CabinClass,
                        SeatType = seatType,
                        IsEmergencyExit = isEmergencyExit,
                        HasExtraLegroom = isEmergencyExit || row == cabin.RowStart,
                        IsActive = true
                    });
                }
            }
        }

        return seats;
    }

    private char[] GetColumnsFromLayout(string layout)
    {
        // Parse layouts like "3-3" or "3-4-3" or "1-2-1" or "2-2"
        return layout switch
        {
            "1-2-1" => ['A', 'D', 'E', 'K'],
            "2-2" => ['A', 'C', 'D', 'F'],
            "2-2-2" => ['A', 'C', 'D', 'E', 'G', 'H'],
            "2-3-2" => ['A', 'C', 'D', 'E', 'F', 'H', 'K'],
            "3-3" => ['A', 'B', 'C', 'D', 'E', 'F'],
            "3-3-3" => ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J'],
            "3-4-3" => ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K'],
            _ => ['A', 'B', 'C', 'D', 'E', 'F']
        };
    }

    private SeatType DetermineSeatType(int columnIndex, int totalColumns, string layout)
    {
        // Simplified seat type determination
        if (layout == "1-2-1")
        {
            // All seats are either window (A,K) or aisle (D,E)
            return columnIndex is 0 or 3 ? SeatType.Window : SeatType.Aisle;
        }

        if (columnIndex == 0 || columnIndex == totalColumns - 1)
            return SeatType.Window;

        // Check if it's an aisle seat based on layout pattern
        return layout switch
        {
            "2-2" => columnIndex is 1 or 2 ? SeatType.Aisle : SeatType.Window,
            "3-3" => columnIndex is 2 or 3 ? SeatType.Aisle : (columnIndex is 1 or 4 ? SeatType.Middle : SeatType.Window),
            "3-4-3" => columnIndex is 2 or 3 or 6 or 7 ? SeatType.Aisle : (columnIndex is 1 or 4 or 5 or 8 ? SeatType.Middle : SeatType.Window),
            _ => SeatType.Middle
        };
    }

    private bool IsEmergencyExitRow(int row, FlightClass cabinClass)
    {
        // Typical emergency exit rows for economy
        if (cabinClass == FlightClass.Economy)
        {
            return row is 14 or 15 or 30 or 31;
        }
        return false;
    }
}

