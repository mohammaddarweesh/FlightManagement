using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Links amenities to flights with class-specific availability and pricing.
/// Defines which amenities are included free vs. available for purchase.
/// </summary>
public class FlightAmenity : BaseEntity
{
    /// <summary>
    /// Foreign key to the flight.
    /// </summary>
    public Guid FlightId { get; set; }

    /// <summary>
    /// Foreign key to the amenity.
    /// </summary>
    public Guid AmenityId { get; set; }

    /// <summary>
    /// Specific cabin class this applies to.
    /// Null means the amenity applies to all classes.
    /// </summary>
    public FlightClass? CabinClass { get; set; }

    /// <summary>
    /// If true, the amenity is included in the ticket price.
    /// If false, it's available for purchase at the specified price.
    /// </summary>
    public bool IsIncluded { get; set; }

    /// <summary>
    /// Price for the amenity if not included.
    /// Null if IsIncluded is true.
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// Currency for the price. Example: "USD"
    /// </summary>
    public string? Currency { get; set; }

    // Navigation properties

    /// <summary>
    /// The flight this amenity is available on.
    /// </summary>
    public Flight Flight { get; set; } = null!;

    /// <summary>
    /// The amenity details.
    /// </summary>
    public Amenity Amenity { get; set; } = null!;
}

