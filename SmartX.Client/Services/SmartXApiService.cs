using System.Net.Http.Json;
using SmartX.Client.Models;

namespace SmartX.Client.Services;

public class SmartXApiService
{
    private readonly HttpClient _httpClient;

    public SmartXApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<SensorDto>> GetSensorsAsync()
    {
        return await _httpClient
            .GetFromJsonAsync<List<SensorDto>>(
                "api/Sensor") ?? [];
    }

    public async Task<SensorDto?> GetSensorAsync(int id)
    {
        return await _httpClient
            .GetFromJsonAsync<SensorDto>(
                $"api/Sensor/{id}");
    }

    public async Task<SensorDto?> CreateSensorAsync(
        SensorDto sensor)
    {
        var response =
            await _httpClient.PostAsJsonAsync(
                "api/Sensor",
                sensor);

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content
            .ReadFromJsonAsync<SensorDto>();
    }

    public async Task<List<TelemetryDto>> GetTelemetryAsync()
    {
        return await _httpClient
            .GetFromJsonAsync<List<TelemetryDto>>(
                "api/Telemetry") ?? [];
    }

    public async Task<List<TelemetryDto>>
        GetTemperatureAsync()
    {
        return await _httpClient
            .GetFromJsonAsync<List<TelemetryDto>>(
                "api/Telemetry/temperature") ?? [];
    }

    public async Task<List<TelemetryDto>>
        GetPowerAsync()
    {
        return await _httpClient
            .GetFromJsonAsync<List<TelemetryDto>>(
                "api/Telemetry/power") ?? [];
    }

    public async Task<List<TelemetryDto>>
        GetActuatorsAsync()
    {
        return await _httpClient
            .GetFromJsonAsync<List<TelemetryDto>>(
                "api/Telemetry/actuators") ?? [];
    }

    public async Task<List<AlertDto>>
        GetAlertsAsync()
    {
        return await _httpClient
            .GetFromJsonAsync<List<AlertDto>>(
                "api/Alert") ?? [];
    }

    public async Task<List<AlertDto>>
        GetActiveAlertsAsync()
    {
        return await _httpClient
            .GetFromJsonAsync<List<AlertDto>>(
                "api/Alert/active") ?? [];
    }

    public async Task<bool> GenerateTelemetryAsync(
        int devices = 1000,
        int readingsPerDevice = 10)
    {
        var response =
            await _httpClient.PostAsync(
                $"api/telemetry/generate" +
                $"?devices={devices}" +
                $"&readingsPerDevice={readingsPerDevice}",
                null);

        return response.IsSuccessStatusCode;
    }
}