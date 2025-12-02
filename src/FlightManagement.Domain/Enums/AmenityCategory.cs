namespace FlightManagement.Domain.Enums;

/// <summary>
/// Categorizes flight amenities and services by type.
/// Used for organizing and filtering available amenities.
/// </summary>
public enum AmenityCategory
{
    /// <summary>
    /// Internet and charging options (WiFi, power outlets, USB ports).
    /// </summary>
    Connectivity = 0,

    /// <summary>
    /// In-flight entertainment options (TV, movies, music, games).
    /// </summary>
    Entertainment = 1,

    /// <summary>
    /// Physical comfort amenities (extra legroom, blankets, pillows, amenity kits).
    /// </summary>
    Comfort = 2,

    /// <summary>
    /// Food and beverage services (meals, snacks, beverages, premium dining).
    /// </summary>
    Dining = 3,

    /// <summary>
    /// Baggage-related services (checked bags, carry-on, priority handling).
    /// </summary>
    Baggage = 4,

    /// <summary>
    /// Priority services (priority boarding, fast track security, lounge access).
    /// </summary>
    Priority = 5
}

