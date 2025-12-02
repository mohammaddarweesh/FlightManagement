using Microsoft.Extensions.Logging;

namespace FlightManagement.Infrastructure.BackgroundJobs;

public class SampleBackgroundJob
{
    private readonly ILogger<SampleBackgroundJob> _logger;

    public SampleBackgroundJob(ILogger<SampleBackgroundJob> logger)
    {
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        _logger.LogInformation("Sample background job started at {Time}", DateTime.UtcNow);
        
        // Your background job logic here
        await Task.Delay(1000);
        
        _logger.LogInformation("Sample background job completed at {Time}", DateTime.UtcNow);
    }
}

