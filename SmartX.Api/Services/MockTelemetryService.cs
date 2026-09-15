using SmartX.Api.Models;

namespace SmartX.Api.Services;

public class MockTelemetryService
{
    private readonly Random _random = new();

    public Task<List<TelemetryPacket<float>>>
        GenerateTemperatureAsync(
            int deviceCount = 1000,
            int readingsPerDevice = 10)
    {
        var results =
            new List<TelemetryPacket<float>>(
                deviceCount * readingsPerDevice);

        for (int device = 1;
             device <= deviceCount;
             device++)
        {
            string deviceId =
                $"ESP32-{device:0000}";

            for (int reading = 0;
                 reading < readingsPerDevice;
                 reading++)
            {
                float temperature =
                    (float)(_random.NextDouble() * 15 + 18);

                // Deliberate anomaly.
                if (_random.NextDouble() < 0.02)
                {
                    temperature =
                        (float)(_random.NextDouble() * 20 + 45);
                }

                results.Add(
                    new TelemetryPacket<float>
                    {
                        DeviceId = deviceId,
                        SensorType = "Temperature",
                        Timestamp =
                            DateTime.UtcNow.AddSeconds(
                                -(
                                    readingsPerDevice -
                                    reading)),
                        Value = temperature
                    });
            }
        }

        return Task.FromResult(results);
    }

    public Task<List<TelemetryPacket<int>>>
        GeneratePowerAsync(
            int deviceCount = 1000,
            int readingsPerDevice = 10)
    {
        var results =
            new List<TelemetryPacket<int>>(
                deviceCount * readingsPerDevice);

        for (int device = 1;
             device <= deviceCount;
             device++)
        {
            string deviceId =
                $"ESP32-{device:0000}";

            for (int reading = 0;
                 reading < readingsPerDevice;
                 reading++)
            {
                int power =
                    _random.Next(100, 1500);

                // Deliberate power anomaly.
                if (_random.NextDouble() < 0.02)
                {
                    power =
                        _random.Next(5000, 10000);
                }

                results.Add(
                    new TelemetryPacket<int>
                    {
                        DeviceId = deviceId,
                        SensorType = "Power",
                        Timestamp =
                            DateTime.UtcNow.AddSeconds(
                                -(
                                    readingsPerDevice -
                                    reading)),
                        Value = power
                    });
            }
        }

        return Task.FromResult(results);
    }

    public Task<List<TelemetryPacket<bool>>>
        GenerateActuatorAsync(
            int deviceCount = 1000,
            int readingsPerDevice = 10)
    {
        var results =
            new List<TelemetryPacket<bool>>(
                deviceCount * readingsPerDevice);

        for (int device = 1;
             device <= deviceCount;
             device++)
        {
            string deviceId =
                $"ESP32-{device:0000}";

            for (int reading = 0;
                 reading < readingsPerDevice;
                 reading++)
            {
                results.Add(
                    new TelemetryPacket<bool>
                    {
                        DeviceId = deviceId,
                        SensorType = "Valve",
                        Timestamp =
                            DateTime.UtcNow.AddSeconds(
                                -(
                                    readingsPerDevice -
                                    reading)),
                        Value =
                            _random.Next(0, 2) == 1
                    });
            }
        }

        return Task.FromResult(results);
    }

    public List<TelemetryPacket<float>>
        GenerateUsingJaggedArrays(
            int deviceCount = 100,
            int readingsPerDevice = 20)
    {
        float[][] temperatureHistory =
            new float[deviceCount][];

        for (int device = 0;
             device < deviceCount;
             device++)
        {
            temperatureHistory[device] =
                new float[readingsPerDevice];

            for (int reading = 0;
                 reading < readingsPerDevice;
                 reading++)
            {
                temperatureHistory[device][reading] =
                    (float)(
                        _random.NextDouble() * 15 + 18);
            }
        }

        var packets =
            new List<TelemetryPacket<float>>();

        for (int device = 0;
             device < deviceCount;
             device++)
        {
            for (int reading = 0;
                 reading < readingsPerDevice;
                 reading++)
            {
                packets.Add(
                    new TelemetryPacket<float>
                    {
                        DeviceId =
                            $"ESP32-{device + 1:0000}",
                        SensorType = "Temperature",
                        Timestamp = DateTime.UtcNow,
                        Value =
                            temperatureHistory[
                                device][reading]
                    });
            }
        }

        return packets;
    }

    public List<TelemetryPacket<float>>
        GenerateUsingMultiDimensionalArray(
            int deviceCount = 100,
            int readingsPerDevice = 20)
    {
        float[,] history =
            new float[
                deviceCount,
                readingsPerDevice];

        for (int device = 0;
             device < deviceCount;
             device++)
        {
            for (int reading = 0;
                 reading < readingsPerDevice;
                 reading++)
            {
                history[device, reading] =
                    (float)(
                        _random.NextDouble() * 15 + 18);
            }
        }

        var packets =
            new List<TelemetryPacket<float>>();

        for (int device = 0;
             device < deviceCount;
             device++)
        {
            for (int reading = 0;
                 reading < readingsPerDevice;
                 reading++)
            {
                packets.Add(
                    new TelemetryPacket<float>
                    {
                        DeviceId =
                            $"ESP32-{device + 1:0000}",
                        SensorType = "Temperature",
                        Timestamp = DateTime.UtcNow,
                        Value =
                            history[device, reading]
                    });
            }
        }

        return packets;
    }
}