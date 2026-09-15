namespace SmartX.Client.Models;

public class SensorDto
{
    public int Id { get; set; }

    public string MacAddress { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public bool IsOnline { get; set; }

    public DateTime RegisteredAt { get; set; }
}
