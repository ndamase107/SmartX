namespace SmartX.Client.Models;

public class TelemetryDto
{
    public string DeviceId { get; set; } = string.Empty;

    public string SensorType { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }

    public object? Value { get; set; }
}