using Microsoft.AspNetCore.Authorization;

namespace FlightManagement.API.Authorization;

/// <summary>
/// Attribute to require Admin user type for access
/// </summary>
public class RequireAdminAttribute : AuthorizeAttribute
{
    public RequireAdminAttribute() : base(AuthorizationPolicies.RequireAdmin)
    {
    }
}

