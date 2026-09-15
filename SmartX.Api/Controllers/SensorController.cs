using Microsoft.AspNetCore.Mvc;
using SmartX.Api.Models;
using SmartX.Api.Services;

namespace SmartX.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SensorController : ControllerBase
{
    private readonly SensorService _sensorService;

    public SensorController(SensorService sensorService)
    {
        _sensorService = sensorService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_sensorService.GetAll());
    }

    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var sensor = _sensorService.GetById(id);

        if (sensor == null)
            return NotFound();

        return Ok(sensor);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Sensor sensor)
    {
        var result = _sensorService.Add(sensor);

        if (!result.Success)
        {
            return BadRequest(new
            {
                message = result.Message
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = result.Sensor!.Id },
            result.Sensor);
    }
}