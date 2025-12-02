namespace FlightManagement.Domain.Enums;

/// <summary>
/// Flags for days of the week. Used for pricing rules and restrictions.
/// </summary>
[Flags]
public enum DayOfWeekFlag
{
    None = 0,
    Sunday = 1,
    Monday = 2,
    Tuesday = 4,
    Wednesday = 8,
    Thursday = 16,
    Friday = 32,
    Saturday = 64,

    /// <summary>
    /// Monday through Friday.
    /// </summary>
    Weekdays = Monday | Tuesday | Wednesday | Thursday | Friday,

    /// <summary>
    /// Saturday and Sunday.
    /// </summary>
    Weekend = Saturday | Sunday,

    /// <summary>
    /// All days.
    /// </summary>
    AllDays = Sunday | Monday | Tuesday | Wednesday | Thursday | Friday | Saturday
}

