using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Master catalog of available amenities and services.
/// Amenities can be linked to flights with class-specific pricing.
/// </summary>
public class Amenity : BaseEntity
{
    /// <summary>
    /// Unique code identifier for the amenity.
    /// Example: "WIFI", "MEAL", "LEGROOM", "LOUNGE"
    /// </summary>
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// Display name for the amenity.
    /// Example: "In-Flight WiFi", "Complimentary Meal"
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the amenity.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Category for organizing and filtering amenities.
    /// </summary>
    public AmenityCategory Category { get; set; }

    /// <summary>
    /// URL to an icon representing this amenity.
    /// Used in UI displays.
    /// </summary>
    public string? IconUrl { get; set; }

    /// <summary>
    /// Indicates if this amenity is active and can be offered.
    /// </summary>
    public bool IsActive { get; set; } = true;

    // Navigation properties

    /// <summary>
    /// Flight-amenity associations.
    /// </summary>
    public ICollection<FlightAmenity> FlightAmenities { get; set; } = new List<FlightAmenity>();
}

