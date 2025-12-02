using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Defines the cabin class configuration for a specific aircraft.
/// Specifies seat count, row range, and layout for each class.
/// </summary>
public class AircraftCabinClass : BaseEntity
{
    /// <summary>
    /// Foreign key to the aircraft.
    /// </summary>
    public Guid AircraftId { get; set; }

    /// <summary>
    /// The cabin class type (Economy, Business, etc.).
    /// </summary>
    public FlightClass CabinClass { get; set; }

    /// <summary>
    /// Total number of seats in this cabin class.
    /// </summary>
    public int SeatCount { get; set; }

    /// <summary>
    /// Starting row number for this cabin class.
    /// Example: First class might start at row 1.
    /// </summary>
    public int RowStart { get; set; }

    /// <summary>
    /// Ending row number for this cabin class.
    /// Example: First class might end at row 4.
    /// </summary>
    public int RowEnd { get; set; }

    /// <summary>
    /// Number of seats per row in this class.
    /// Example: 6 for economy (3-3), 4 for business (2-2).
    /// </summary>
    public int SeatsPerRow { get; set; }

    /// <summary>
    /// Visual representation of seat layout.
    /// Example: "3-3" for 6-abreast, "2-4-2" for wide-body economy, "1-2-1" for business.
    /// </summary>
    public string SeatLayout { get; set; } = string.Empty;

    /// <summary>
    /// Base price multiplier for this class relative to economy.
    /// Example: Economy = 1.0, Business = 3.0, First = 5.0
    /// </summary>
    public decimal BasePriceMultiplier { get; set; } = 1.0m;

    // Navigation properties

    /// <summary>
    /// The aircraft this configuration belongs to.
    /// </summary>
    public Aircraft Aircraft { get; set; } = null!;
}

