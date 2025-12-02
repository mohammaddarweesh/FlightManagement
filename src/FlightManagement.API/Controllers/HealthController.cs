using Microsoft.AspNetCore.Mvc;

namespace FlightManagement.API.Controllers;

public class HealthController : BaseApiController
{
    private readonly ILogger<HealthController> _logger;

    public HealthController(ILogger<HealthController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Health check endpoint called");

        return Ok(new
        {
            Status = "Healthy",
            Timestamp = DateTime.UtcNow,
            Service = "FlightManagement API"
        });
    }
}

