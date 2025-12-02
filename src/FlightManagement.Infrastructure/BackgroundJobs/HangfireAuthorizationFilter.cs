using Hangfire.Dashboard;

namespace FlightManagement.Infrastructure.BackgroundJobs;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // TODO: Implement proper authorization
        // For now, allow all requests (development only)
        return true;
    }
}

