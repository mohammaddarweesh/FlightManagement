using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Represents a scheduled flight on a specific date.
/// Core entity containing route, timing, and operational status.
/// </summary>
public class Flight : BaseEntity
{
    /// <summary>
    /// Flight number identifier. Example: "AA100", "BA2490"
    /// Combination of airline code and number.
    /// </summary>
    public string FlightNumber { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to the operating airline.
    /// </summary>
    public Guid AirlineId { get; set; }

    /// <summary>
    /// Foreign key to the assigned aircraft.
    /// </summary>
    public Guid AircraftId { get; set; }

    /// <summary>
    /// Foreign key to the departure airport.
    /// </summary>
    public Guid DepartureAirportId { get; set; }

    /// <summary>
    /// Foreign key to the arrival airport.
    /// </summary>
    public Guid ArrivalAirportId { get; set; }

    /// <summary>
    /// Scheduled departure date and time in UTC.
    /// </summary>
    public DateTime ScheduledDepartureTime { get; set; }

    /// <summary>
    /// Scheduled arrival date and time in UTC.
    /// </summary>
    public DateTime ScheduledArrivalTime { get; set; }

    /// <summary>
    /// Actual departure time (for tracking delays). Null if not yet departed.
    /// </summary>
    public DateTime? ActualDepartureTime { get; set; }

    /// <summary>
    /// Actual arrival time. Null if not yet arrived.
    /// </summary>
    public DateTime? ActualArrivalTime { get; set; }

    /// <summary>
    /// Departure terminal at the origin airport. Example: "T4", "Terminal 2"
    /// </summary>
    public string? DepartureTerminal { get; set; }

    /// <summary>
    /// Arrival terminal at the destination airport.
    /// </summary>
    public string? ArrivalTerminal { get; set; }

    /// <summary>
    /// Departure gate. Example: "B22", "Gate 15"
    /// </summary>
    public string? DepartureGate { get; set; }

    /// <summary>
    /// Arrival gate at destination.
    /// </summary>
    public string? ArrivalGate { get; set; }

    /// <summary>
    /// Current operational status of the flight.
    /// </summary>
    public FlightStatus Status { get; set; } = FlightStatus.Scheduled;

    /// <summary>
    /// Calculated flight duration.
    /// </summary>
    public TimeSpan Duration { get; set; }

    /// <summary>
    /// Distance of the route in kilometers.
    /// </summary>
    public int? DistanceKm { get; set; }

    /// <summary>
    /// Indicates if the flight is active and bookable.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties

    /// <summary>
    /// The airline operating this flight.
    /// </summary>
    public Airline Airline { get; set; } = null!;

    /// <summary>
    /// The aircraft assigned to this flight.
    /// </summary>
    public Aircraft Aircraft { get; set; } = null!;

    /// <summary>
    /// The departure airport.
    /// </summary>
    public Airport DepartureAirport { get; set; } = null!;

    /// <summary>
    /// The arrival airport.
    /// </summary>
    public Airport ArrivalAirport { get; set; } = null!;

    /// <summary>
    /// Pricing information for each cabin class.
    /// </summary>
    public ICollection<FlightPricing> FlightPricings { get; set; } = new List<FlightPricing>();

    /// <summary>
    /// Individual seat availability for this flight.
    /// </summary>
    public ICollection<FlightSeat> FlightSeats { get; set; } = new List<FlightSeat>();

    /// <summary>
    /// Amenities available on this flight.
    /// </summary>
    public ICollection<FlightAmenity> FlightAmenities { get; set; } = new List<FlightAmenity>();

    /// <summary>
    /// Booking segments for this flight.
    /// </summary>
    public ICollection<BookingSegment> BookingSegments { get; set; } = new List<BookingSegment>();
}

