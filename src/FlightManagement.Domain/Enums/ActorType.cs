namespace FlightManagement.Domain.Enums;

/// <summary>
/// Represents the type of actor performing an action.
/// </summary>
public enum ActorType
{
    /// <summary>
    /// Action performed by a customer.
    /// </summary>
    Customer = 0,

    /// <summary>
    /// Action performed by an admin.
    /// </summary>
    Admin = 1,

    /// <summary>
    /// Action performed by the system automatically.
    /// </summary>
    System = 2,

    /// <summary>
    /// Action triggered by external API/integration.
    /// </summary>
    External = 3
}

