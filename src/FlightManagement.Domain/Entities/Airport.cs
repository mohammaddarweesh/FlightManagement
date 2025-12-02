using FlightManagement.Domain.Common;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Represents an airport that serves as departure or arrival location for flights.
/// Contains geographic and operational information.
/// </summary>
public class Airport : BaseEntity
{
    /// <summary>
    /// IATA airport code (3 characters). Example: "JFK", "LHR", "DXB"
    /// </summary>
    public string IataCode { get; set; } = string.Empty;

    /// <summary>
    /// ICAO airport code (4 characters). Example: "KJFK", "EGLL", "OMDB"
    /// </summary>
    public string IcaoCode { get; set; } = string.Empty;

    /// <summary>
    /// Full official name of the airport.
    /// Example: "John F. Kennedy International Airport"
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// City where the airport is located.
    /// </summary>
    public string City { get; set; } = string.Empty;

    /// <summary>
    /// Country where the airport is located.
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// ISO 3166-1 alpha-2 country code. Example: "US", "GB", "AE"
    /// </summary>
    public string CountryCode { get; set; } = string.Empty;

    /// <summary>
    /// IANA timezone identifier. Example: "America/New_York"
    /// Used for accurate departure/arrival time display.
    /// </summary>
    public string Timezone { get; set; } = string.Empty;

    /// <summary>
    /// Geographic latitude coordinate.
    /// </summary>
    public decimal Latitude { get; set; }

    /// <summary>
    /// Geographic longitude coordinate.
    /// </summary>
    public decimal Longitude { get; set; }

    /// <summary>
    /// Indicates if the airport is currently active and accepting flights.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties

    /// <summary>
    /// Flights departing from this airport.
    /// </summary>
    public ICollection<Flight> DepartureFlights { get; set; } = new List<Flight>();

    /// <summary>
    /// Flights arriving at this airport.
    /// </summary>
    public ICollection<Flight> ArrivalFlights { get; set; } = new List<Flight>();
}

