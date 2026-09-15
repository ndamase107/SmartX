namespace SmartX.Api.Models;

public class Sensor
{
    public int Id { get; set; }

    public string MacAddress { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public SensorCategory Category { get; set; }

    public bool IsOnline { get; set; }

    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
}