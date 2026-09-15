using Microsoft.AspNetCore.Mvc;
using SmartX.Api.Services;

namespace SmartX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TelemetryController : ControllerBase
{
    private readonly TelemetryService _telemetryService;

    public TelemetryController(TelemetryService telemetryService)
    {
        _telemetryService = telemetryService;
    }

    [HttpPost("/api/telemetry/generate")]
    public async Task<IActionResult> Generate(
        [FromQuery] int devices = 1000,
        [FromQuery] int readingsPerDevice = 10)
    {
        if (devices < 1 || devices > 5000)
        {
            return BadRequest(
                "Devices must be between 1 and 5000.");
        }

        if (readingsPerDevice < 1 ||
            readingsPerDevice > 100)
        {
            return BadRequest(
                "Readings per device must be between 1 and 100.");
        }

        await _telemetryService.GenerateTelemetryAsync(
            devices,
            readingsPerDevice);

        return Ok(_telemetryService.GetSummary());
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var temperature =
            _telemetryService.GetTemperatureData();

        var power =
            _telemetryService.GetPowerData();

        var actuators =
            _telemetryService.GetActuatorData();

        var result = new List<object>();

        result.AddRange(
            temperature.Select(x => new
            {
                x.DeviceId,
                x.SensorType,
                x.Timestamp,
                Value = (object)x.Value
            }));

        result.AddRange(
            power.Select(x => new
            {
                x.DeviceId,
                x.SensorType,
                x.Timestamp,
                Value = (object)x.Value
            }));

        result.AddRange(
            actuators.Select(x => new
            {
                x.DeviceId,
                x.SensorType,
                x.Timestamp,
                Value = (object)x.Value
            }));

        return Ok(result);
    }

    [HttpGet("temperature")]
    public IActionResult GetTemperature()
    {
        return Ok(
            _telemetryService.GetTemperatureData());
    }

    [HttpGet("power")]
    public IActionResult GetPower()
    {
        return Ok(
            _telemetryService.GetPowerData());
    }

    [HttpGet("actuators")]
    public IActionResult GetActuators()
    {
        return Ok(
            _telemetryService.GetActuatorData());
    }

    [HttpGet("summary")]
    public IActionResult GetSummary()
    {
        return Ok(
            _telemetryService.GetSummary());
    }

    [HttpGet("delta")]
    public IActionResult GetDelta()
    {
        var delta =
            _telemetryService.CalculateTemperatureDelta();

        return Ok(new
        {
            value = delta.Value,
            unit = delta.Unit
        });
    }
}