using FlightManagement.Domain.Common;
using FlightManagement.Domain.Enums;

namespace FlightManagement.Domain.Entities;

/// <summary>
/// Represents an individual traveler on a booking.
/// Contains identity documents and preferences for air travel.
/// </summary>
public class Passenger : BaseEntity
{
    /// <summary>
    /// Foreign key to the booking.
    /// </summary>
    public Guid BookingId { get; set; }

    /// <summary>
    /// Type of passenger (Adult, Child, Infant).
    /// </summary>
    public PassengerType PassengerType { get; set; }

    // Identity information

    /// <summary>
    /// Title (Mr, Mrs, Ms, Dr, etc.).
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// First name as on passport.
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// Middle name as on passport (optional).
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Last name as on passport.
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// Date of birth.
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gender.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Nationality (ISO country code).
    /// </summary>
    public string Nationality { get; set; } = string.Empty;

    // Travel documents

    /// <summary>
    /// Passport number.
    /// </summary>
    public string? PassportNumber { get; set; }

    /// <summary>
    /// Passport issuing country (ISO country code).
    /// </summary>
    public string? PassportIssuingCountry { get; set; }

    /// <summary>
    /// Passport expiry date.
    /// </summary>
    public DateTime? PassportExpiryDate { get; set; }

    // Contact information

    /// <summary>
    /// Email address for notifications.
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// Phone number for SMS updates.
    /// </summary>
    public string? Phone { get; set; }

    // Preferences

    /// <summary>
    /// Meal preference for in-flight dining.
    /// </summary>
    public MealPreference? MealPreference { get; set; }

    /// <summary>
    /// Special assistance requirements.
    /// </summary>
    public string? SpecialAssistance { get; set; }

    /// <summary>
    /// Frequent flyer number.
    /// </summary>
    public string? FrequentFlyerNumber { get; set; }

    /// <summary>
    /// Foreign key to the airline for frequent flyer.
    /// </summary>
    public Guid? FrequentFlyerAirlineId { get; set; }

    // Flags

    /// <summary>
    /// Indicates if this is the primary contact person.
    /// </summary>
    public bool IsPrimaryContact { get; set; }

    /// <summary>
    /// Indicates if this is the lead passenger.
    /// </summary>
    public bool IsLeadPassenger { get; set; }

    // Navigation properties

    public Booking Booking { get; set; } = null!;
    public Airline? FrequentFlyerAirline { get; set; }
    public ICollection<PassengerSeat> PassengerSeats { get; set; } = new List<PassengerSeat>();
}

