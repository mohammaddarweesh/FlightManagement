namespace FlightManagement.Domain.Enums;

/// <summary>
/// Types of extra services that can be added to a booking.
/// </summary>
public enum ExtraType
{
    /// <summary>
    /// Additional checked baggage.
    /// </summary>
    ExtraBaggage = 0,

    /// <summary>
    /// Priority boarding service.
    /// </summary>
    PriorityBoarding = 1,

    /// <summary>
    /// Travel insurance.
    /// </summary>
    Insurance = 2,

    /// <summary>
    /// Upgraded meal option.
    /// </summary>
    MealUpgrade = 3,

    /// <summary>
    /// Airport lounge access.
    /// </summary>
    LoungeAccess = 4,

    /// <summary>
    /// Fast track security.
    /// </summary>
    FastTrack = 5,

    /// <summary>
    /// In-flight WiFi package.
    /// </summary>
    WiFi = 6,

    /// <summary>
    /// Premium seat selection (exit row, extra legroom).
    /// </summary>
    PremiumSeat = 7,

    /// <summary>
    /// Pet in cabin.
    /// </summary>
    PetInCabin = 8,

    /// <summary>
    /// Sports equipment (golf clubs, skis, etc.).
    /// </summary>
    SportsEquipment = 9,

    /// <summary>
    /// Airport transfer service.
    /// </summary>
    AirportTransfer = 10,

    /// <summary>
    /// Other/miscellaneous extra.
    /// </summary>
    Other = 99
}

