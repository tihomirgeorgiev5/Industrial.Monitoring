using Industrial.Monitoring.Api.Models;
using MQTTnet;
using MQTTnet.Client;
using System.Text;
using System.Text.Json;

namespace Industrial.Monitoring.Api.Services
{
    public class TelemetryIngestService : BackgroundService
    {
        private readonly ILogger<TelemetryIngestService> _logger;
        private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
        public TelemetryIngestService(ILogger<TelemetryIngestService> logger)
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

            client.ApplicationMessageReceivedAsync += e =>
            {
                var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

                var msg = JsonSerializer.Deserialize<TelemetryMessage>(payload, JsonOptions);
                if (msg is null) return Task.CompletedTask;

                _logger.LogInformation("{MachineId}: {Rpm} rpm, {Temp} °C, {Pressure} bar",
                    msg.MachineId, msg.Rpm, msg.Temperature, msg.Pressure);

                return Task.CompletedTask;
            };

            await client.SubscribeAsync(
                factory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter("machines/+/telemetry")
                    .Build(), ct);

            await Task.Delay(Timeout.Infinite, ct);


        }
    }
}
