using Microsoft.AspNetCore.Mvc;
using SmartX.Api.Services;

namespace SmartX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertController : ControllerBase
{
    private readonly TelemetryService _telemetryService;

    public AlertController(TelemetryService telemetryService)
    {
        _telemetryService = telemetryService;
    }

    [HttpGet]
    public IActionResult GetAlerts()
    {
        return Ok(_telemetryService.GetAlerts());
    }

    [HttpGet("active")]
    public IActionResult GetActiveAlerts()
    {
        var alerts = _telemetryService
            .GetAlerts()
            .Where(x => !x.IsResolved)
            .OrderByDescending(x => x.Timestamp)
            .ToList();

        return Ok(alerts);
    }
}