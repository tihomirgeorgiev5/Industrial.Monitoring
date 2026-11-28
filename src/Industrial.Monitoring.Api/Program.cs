using Industrial.Monitoring.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<MachineSimulator>();

builder.Services.AddHostedService<TelemetryIngestService>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
