using SmartX.Api.Collections;
using SmartX.Api.Models;

namespace SmartX.Api.Services;

public class TelemetryService
{
    private readonly MockTelemetryService _mockTelemetry;
    private readonly AnomalyDetectionService _anomalyDetection;

    private readonly TelemetryCollection<
        TelemetryPacket<float>> _temperatureData = new();

    private readonly TelemetryCollection<
        TelemetryPacket<int>> _powerData = new();

    private readonly TelemetryCollection<
        TelemetryPacket<bool>> _actuatorData = new();

    private readonly List<Alert> _alerts = [];

    public TelemetryService(
        MockTelemetryService mockTelemetry,
        AnomalyDetectionService anomalyDetection)
    {
        _mockTelemetry = mockTelemetry;
        _anomalyDetection = anomalyDetection;
    }

    public async Task GenerateTelemetryAsync(
        int devices = 1000,
        int readingsPerDevice = 10)
    {
        _temperatureData.Clear();
        _powerData.Clear();
        _actuatorData.Clear();
        _alerts.Clear();

        var temperatures =
            await _mockTelemetry
                .GenerateTemperatureAsync(
                    devices,
                    readingsPerDevice);

        var power =
            await _mockTelemetry
                .GeneratePowerAsync(
                    devices,
                    readingsPerDevice);

        var actuators =
            await _mockTelemetry
                .GenerateActuatorAsync(
                    devices,
                    readingsPerDevice);

        _temperatureData.AddRange(temperatures);
        _powerData.AddRange(power);
        _actuatorData.AddRange(actuators);

        _alerts.AddRange(
            _anomalyDetection
                .DetectTemperatureAnomalies(
                    temperatures));

        _alerts.AddRange(
            _anomalyDetection
                .DetectPowerAnomalies(
                    power));

        SimulateDisconnectedSensors(devices);
    }

    private void SimulateDisconnectedSensors(
        int deviceCount)
    {
        for (int device = 100;
             device <= deviceCount;
             device += 100)
        {
            _alerts.Add(
                new Alert
                {
                    DeviceId =
                        $"ESP32-{device:0000}",

                    Message =
                        "Sensor disconnected or no telemetry received.",

                    Severity = "Critical",

                    Timestamp = DateTime.UtcNow,

                    IsResolved = false
                });
        }
    }

    public IReadOnlyList<
        TelemetryPacket<float>>
        GetTemperatureData()
    {
        return _temperatureData.Items;
    }

    public IReadOnlyList<
        TelemetryPacket<int>>
        GetPowerData()
    {
        return _powerData.Items;
    }

    public IReadOnlyList<
        TelemetryPacket<bool>>
        GetActuatorData()
    {
        return _actuatorData.Items;
    }

    public IReadOnlyList<Alert> GetAlerts()
    {
        return _alerts;
    }

    public object GetSummary()
    {
        return new
        {
            TemperatureRecords =
                _temperatureData.Count,

            PowerRecords =
                _powerData.Count,

            ActuatorRecords =
                _actuatorData.Count,

            TotalRecords =
                _temperatureData.Count +
                _powerData.Count +
                _actuatorData.Count,

            ActiveAlerts =
                _alerts.Count
        };
    }

    public SensorValue CalculateTemperatureDelta()
    {
        if (_temperatureData.Count < 2)
        {
            return new SensorValue(
                0,
                "°C");
        }

        var previous =
            _temperatureData[
                _temperatureData.Count - 2];

        var current =
            _temperatureData[
                _temperatureData.Count - 1];

        var previousValue =
            new SensorValue(
                previous.Value,
                "°C");

        var currentValue =
            new SensorValue(
                current.Value,
                "°C");

        return currentValue - previousValue;
    }
}