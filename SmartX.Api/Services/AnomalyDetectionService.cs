using SmartX.Api.Models;

namespace SmartX.Api.Services;

public class AnomalyDetectionService
{
    public Alert? CheckTemperature(
        TelemetryPacket<float> packet)
    {
        const float minimum = 10;
        const float maximum = 40;

        if (packet.Value < minimum ||
            packet.Value > maximum)
        {
            return new Alert
            {
                DeviceId = packet.DeviceId,

                Message =
                    $"Temperature anomaly detected: " +
                    $"{packet.Value:F2} °C",

                Severity =
                    packet.Value > 50
                        ? "Critical"
                        : "Warning",

                Timestamp = packet.Timestamp,

                IsResolved = false
            };
        }

        return null;
    }

    public Alert? CheckPower(
        TelemetryPacket<int> packet)
    {
        const int maximum = 3000;

        if (packet.Value > maximum)
        {
            return new Alert
            {
                DeviceId = packet.DeviceId,

                Message =
                    $"Power spike detected: " +
                    $"{packet.Value} W",

                Severity =
                    packet.Value > 7000
                        ? "Critical"
                        : "Warning",

                Timestamp = packet.Timestamp,

                IsResolved = false
            };
        }

        return null;
    }

    public List<Alert>
        DetectTemperatureAnomalies(
            IEnumerable<TelemetryPacket<float>> packets)
    {
        var alerts = new List<Alert>();

        foreach (var packet in packets)
        {
            var alert =
                CheckTemperature(packet);

            if (alert != null)
            {
                alerts.Add(alert);
            }
        }

        return alerts;
    }

    public List<Alert>
        DetectPowerAnomalies(
            IEnumerable<TelemetryPacket<int>> packets)
    {
        var alerts = new List<Alert>();

        foreach (var packet in packets)
        {
            var alert =
                CheckPower(packet);

            if (alert != null)
            {
                alerts.Add(alert);
            }
        }

        return alerts;
    }
}