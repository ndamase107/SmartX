namespace SmartX.Api.Models;

public class Alert
{
    public int Id { get; set; }

    public string DeviceId { get; set; }
        = string.Empty;

    public string Message { get; set; }
        = string.Empty;

    public string Severity { get; set; }
        = "Warning";

    public DateTime Timestamp { get; set; }
        = DateTime.UtcNow;

    public bool IsResolved { get; set; }
}