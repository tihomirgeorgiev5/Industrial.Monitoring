using Industrial.Monitoring.Api.Models;
using MQTTnet;
using MQTTnet.Client;
using System.Text.Json;

namespace Industrial.Monitoring.Api.Services
{
    public class MachineSimulator : BackgroundService
    {
        private readonly ILogger<MachineSimulator> _logger;
        private readonly Random _random = new();
        public MachineSimulator(ILogger<MachineSimulator> logger)
        {
            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken ct)
        {
            var factory = new MqttFactory();
            using var client = factory.CreateMqttClient();

            var options = new MqttClientOptionsBuilder()
                .WithTcpServer("localhost", 1883)
                .Build();

            await client.ConnectAsync(options, ct);

            _logger.LogInformation("Connected to MQTT broker");


            while (!ct.IsCancellationRequested)
            {
                foreach (var item in MachineCatalog.All)
                {
                    var msg = new TelemetryMessage(
                     item.Id,
                     item.Name,
                     Math.Round(item.Rpm + Noise(item.Rpm * 0.03)),
                     Math.Round(item.Temperature + Noise(1.0),1),
                     Math.Round(item.Pressure + Noise(0.2),2),
                     DateTime.UtcNow);

                    var json = JsonSerializer.Serialize(msg, new JsonSerializerOptions(JsonSerializerDefaults.Web));

                    var mqttMessage = new MqttApplicationMessageBuilder()
                        .WithTopic($"machines/{item.Id}/telemetry")
                        .WithPayload(json)
                        .Build();

                    await client.PublishAsync(mqttMessage, ct);
                }
                await Task.Delay(TimeSpan.FromSeconds(1), ct);
            }

        }
        private double Noise(double amplitude)
        {
            return (_random.NextDouble() - 0.5) * 2 * amplitude;
        }
    }   
}
