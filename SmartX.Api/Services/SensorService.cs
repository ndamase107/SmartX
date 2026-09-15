using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using SmartX.Api.Models;

namespace SmartX.Api.Services;

public class SensorService
{
    private readonly List<Sensor> _sensors = [];

    public IReadOnlyList<Sensor> GetAll()
    {
        return _sensors;
    }

    public Sensor? GetById(int id)
    {
        return _sensors.FirstOrDefault(x => x.Id == id);
    }

    public (bool Success, string Message, Sensor? Sensor)
        Add(Sensor sensor)
    {
        if (!IsValidMacAddress(sensor.MacAddress))
        {
            return (false,
                "Invalid MAC address or unique device ID.",
                null);
        }

        if (string.IsNullOrWhiteSpace(sensor.Location))
        {
            return (false,
                "Deployment location is required.",
                null);
        }

        if (!Enum.IsDefined(sensor.Category))
        {
            return (false,
                "Invalid sensor category.",
                null);
        }

        if (_sensors.Any(x =>
            x.MacAddress.Equals(
                sensor.MacAddress,
                StringComparison.OrdinalIgnoreCase)))
        {
            return (false,
                "A sensor with this MAC address already exists.",
                null);
        }

        sensor.Id =
            _sensors.Count == 0
                ? 1
                : _sensors.Max(x => x.Id) + 1;

        sensor.RegisteredAt = DateTime.UtcNow;
        sensor.IsOnline = true;

        _sensors.Add(sensor);

        return (true, "Sensor registered successfully.", sensor);
    }

    private bool IsValidMacAddress(string mac)
    {
        if (string.IsNullOrWhiteSpace(mac))
            return false;

        /*
         * Accept normal MAC format:
         * AA:BB:CC:DD:EE:FF
         */
        return Regex.IsMatch(
            mac.Trim(),
            "^([0-9A-Fa-f]{2}:){5}[0-9A-Fa-f]{2}$");
    }
}