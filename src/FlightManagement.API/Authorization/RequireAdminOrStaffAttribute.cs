using Microsoft.AspNetCore.Authorization;

namespace FlightManagement.API.Authorization;

/// <summary>
/// Attribute to require Admin or Staff user type for access
/// </summary>
public class RequireAdminOrStaffAttribute : AuthorizeAttribute
{
    public RequireAdminOrStaffAttribute() : base(AuthorizationPolicies.RequireAdminOrStaff)
    {
    }
}

