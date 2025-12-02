using FlightManagement.Domain.Common;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Represents a physical aircraft with its specifications and seat configuration.
/// Each aircraft belongs to an airline and has a unique registration number.
/// </summary>
public class Aircraft : BaseEntity
{
    /// <summary>
    /// Foreign key to the owning/operating airline.
    /// </summary>
    public Guid AirlineId { get; set; }

    /// <summary>
    /// Aircraft model name. Example: "Boeing 737-800", "Airbus A380-800"
    /// </summary>
    public string Model { get; set; } = string.Empty;

    /// <summary>
    /// Aircraft manufacturer. Example: "Boeing", "Airbus", "Embraer"
    /// </summary>
    public string Manufacturer { get; set; } = string.Empty;

    /// <summary>
    /// Unique aircraft registration number. Example: "N12345", "G-XWBA"
    /// This is the aircraft's "license plate".
    /// </summary>
    public string RegistrationNumber { get; set; } = string.Empty;

    /// <summary>
    /// Total number of passenger seats on the aircraft.
    /// Sum of all cabin class capacities.
    /// </summary>
    public int TotalSeats { get; set; }

    /// <summary>
    /// Year the aircraft was manufactured.
    /// </summary>
    public int? ManufactureYear { get; set; }

    /// <summary>
    /// Maximum range in kilometers the aircraft can fly.
    /// </summary>
    public int? RangeKm { get; set; }

    /// <summary>
    /// Cruising speed in km/h.
    /// </summary>
    public int? CruisingSpeedKmh { get; set; }

    /// <summary>
    /// Indicates if the aircraft is currently active and available for flights.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties

    /// <summary>
    /// The airline that owns/operates this aircraft.
    /// </summary>
    public Airline Airline { get; set; } = null!;

    /// <summary>
    /// Cabin class configurations for this aircraft.
    /// Defines seat counts and layouts per class.
    /// </summary>
    public ICollection<AircraftCabinClass> CabinClasses { get; set; } = new List<AircraftCabinClass>();

    /// <summary>
    /// Individual seats on this aircraft.
    /// </summary>
    public ICollection<Seat> Seats { get; set; } = new List<Seat>();

    /// <summary>
    /// Flights assigned to this aircraft.
    /// </summary>
    public ICollection<Flight> Flights { get; set; } = new List<Flight>();
}

