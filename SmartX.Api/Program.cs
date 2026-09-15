using SmartX.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSingleton<SensorService>();
builder.Services.AddSingleton<MockTelemetryService>();
builder.Services.AddSingleton<AnomalyDetectionService>();
builder.Services.AddSingleton<TelemetryService>();
builder.Services.AddSingleton<FileService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "SmartXClient",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("SmartXClient");

app.MapControllers();

app.Run();

