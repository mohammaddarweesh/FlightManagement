using Microsoft.AspNetCore.Authorization;

namespace FlightManagement.API.Authorization;

/// <summary>
/// Attribute to require Customer user type for access
/// </summary>
public class RequireCustomerAttribute : AuthorizeAttribute
{
    public RequireCustomerAttribute() : base(AuthorizationPolicies.RequireCustomer)
    {
    }
}

