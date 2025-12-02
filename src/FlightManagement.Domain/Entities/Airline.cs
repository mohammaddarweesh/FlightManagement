using FlightManagement.Domain.Common;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Represents an airline company that operates flights.
/// Contains airline identification and branding information.
/// </summary>
public class Airline : BaseEntity
{
    /// <summary>
    /// IATA airline code (2 characters). Example: "AA", "BA", "EK"
    /// </summary>
    public string IataCode { get; set; } = string.Empty;

    /// <summary>
    /// ICAO airline code (3 characters). Example: "AAL", "BAW", "UAE"
    /// </summary>
    public string IcaoCode { get; set; } = string.Empty;

    /// <summary>
    /// Full name of the airline. Example: "American Airlines"
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Country where the airline is headquartered.
    /// </summary>
    public string Country { get; set; } = string.Empty;

    /// <summary>
    /// URL to the airline's logo image.
    /// Used for display in search results and booking pages.
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Indicates if the airline is currently active and operating.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties

    /// <summary>
    /// Aircraft owned/operated by this airline.
    /// </summary>
    public ICollection<Aircraft> Aircraft { get; set; } = new List<Aircraft>();

    /// <summary>
    /// Flights operated by this airline.
    /// </summary>
    public ICollection<Flight> Flights { get; set; } = new List<Flight>();

    /// <summary>
    /// Passengers with frequent flyer accounts for this airline.
    /// </summary>
    public ICollection<Passenger> FrequentFlyers { get; set; } = new List<Passenger>();

    /// <summary>
    /// Overbooking policies for this airline.
    /// </summary>
    public ICollection<OverbookingPolicy> OverbookingPolicies { get; set; } = new List<OverbookingPolicy>();
}

