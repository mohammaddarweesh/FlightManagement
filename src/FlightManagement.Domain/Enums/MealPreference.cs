namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents dietary preferences for in-flight meals.
/// </summary>
public enum MealPreference
{
    /// <summary>
    /// Standard meal with no restrictions.
    /// </summary>
    Regular = 0,

    /// <summary>
    /// Vegetarian meal (no meat or fish).
    /// </summary>
    Vegetarian = 1,

    /// <summary>
    /// Vegan meal (no animal products).
    /// </summary>
    Vegan = 2,

    /// <summary>
    /// Halal meal prepared according to Islamic law.
    /// </summary>
    Halal = 3,

    /// <summary>
    /// Kosher meal prepared according to Jewish law.
    /// </summary>
    Kosher = 4,

    /// <summary>
    /// Gluten-free meal.
    /// </summary>
    GlutenFree = 5,

    /// <summary>
    /// Low-sodium/salt meal.
    /// </summary>
    LowSodium = 6,

    /// <summary>
    /// Diabetic-friendly meal.
    /// </summary>
    Diabetic = 7,

    /// <summary>
    /// Hindu meal (no beef).
    /// </summary>
    Hindu = 8,

    /// <summary>
    /// Seafood meal.
    /// </summary>
    Seafood = 9,

    /// <summary>
    /// Child-friendly meal.
    /// </summary>
    Child = 10,

    /// <summary>
    /// No meal requested.
    /// </summary>
    None = 11
}

