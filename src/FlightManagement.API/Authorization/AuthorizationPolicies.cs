namespace FlightManagement.API.Authorization;

/// <summary>
/// Contains authorization policy names
/// </summary>
public static class AuthorizationPolicies
{
    /// <summary>
    /// Policy requiring Admin user type
    /// </summary>
    public const string RequireAdmin = "RequireAdmin";

    /// <summary>
    /// Policy requiring Customer user type
    /// </summary>
    public const string RequireCustomer = "RequireCustomer";

    /// <summary>
    /// Policy requiring Admin or Staff user type
    /// </summary>
    public const string RequireAdminOrStaff = "RequireAdminOrStaff";
}

