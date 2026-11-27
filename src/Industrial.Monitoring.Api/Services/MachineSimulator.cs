using Industrial.Monitoring.Api.Models;
using MQTTnet;
using MQTTnet.Client;

namespace Industrial.Monitoring.Api.Services
{
    public class MachineSimulator : BackgroundService
    {
        private readonly ILogger<MachineSimulator> _logger;
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
        }
    }
}
