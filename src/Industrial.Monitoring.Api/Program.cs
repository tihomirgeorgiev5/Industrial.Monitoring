using Industrial.Monitoring.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<MachineSimulator>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
