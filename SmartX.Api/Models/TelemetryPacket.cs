namespace SmartX.Api.Models;

public class TelemetryPacket<T>
{
    public string DeviceId { get; set; } = string.Empty;

    public string SensorType { get; set; } = string.Empty;

    public DateTime Timestamp { get; set; }
        = DateTime.UtcNow;

    public T Value { get; set; } = default!;
}