namespace SmartX.Client.Models;

public class AlertDto
{
    public int Id { get; set; }

    public string DeviceId { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public string Severity { get; set; } = "Warning";

    public DateTime Timestamp { get; set; }

    public bool IsResolved { get; set; }
}