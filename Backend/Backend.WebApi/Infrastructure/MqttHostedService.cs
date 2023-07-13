using System.Globalization;
using System.Text;
using System.Text.Json;
using Backend.WebApi.Configuration;
using Backend.WebApi.Domain.Models;
using Backend.WebApi.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Server;

namespace Backend.WebApi.Infrastructure;

public class MqttHostedService : IHostedService, IDisposable
{
    private MqttServer _mqttServer = null!;
    private ILogger<MqttHostedService>? _logger;
    private readonly MqttHostedServiceOptions _options;
    private readonly ISensorInfoRepository _sensorInfoRepository;
    private readonly IServiceProvider _serviceProvider;

    public MqttHostedService(ILogger<MqttHostedService>? logger, IOptions<MqttHostedServiceOptions> options, ISensorInfoRepository sensorInfoRepository, IServiceProvider serviceProvider)
    {
        _logger = logger;
        _options = options.Value;
        _sensorInfoRepository = sensorInfoRepository;
        _serviceProvider = serviceProvider;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Mqtt Server...");
        var optionsBuilder = new MqttServerOptionsBuilder()
            .WithDefaultEndpoint()
            .WithDefaultEndpointPort(_options.ServerPort);                                                                                                                                                                                                                                                      

        _mqttServer = new MqttFactory().CreateMqttServer(optionsBuilder.Build());
        _mqttServer.InterceptingPublishAsync += MqttServerOnInterceptingPublishAsync;
        await _mqttServer.StartAsync();
    }

    private async Task MqttServerOnInterceptingPublishAsync(InterceptingPublishEventArgs arg)
    {
        try
        {
            if (arg.ApplicationMessage.Topic == _options.Topic)
            {
                string payload = Encoding.UTF8.GetString(arg.ApplicationMessage.PayloadSegment);

                NumberFormatInfo numberFormatInfo = new NumberFormatInfo { NumberDecimalSeparator = "." };

                if (float.TryParse(payload, NumberStyles.Float, numberFormatInfo, out float result))
                {
                    _logger?.LogInformation("Parsed value: {Value}", result);

                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var sensorRepository = scope.ServiceProvider.GetRequiredService<ISensorRepository>();

                        var sensors = await sensorRepository.GetAllAsync();
                        Sensor sensor = sensors.First();

                        SensorInfo info = new SensorInfo()
                        {
                            ID = sensor.ID,
                            Name = sensor.Name,
                            Weight = result
                        };

                        _logger?.LogInformation("Received SensorInfo {SensorInfo}", JsonSerializer.Serialize(info));
                        await _sensorInfoRepository.PushSensorInfo(info).ConfigureAwait(false);
                    }
                }
                else
                {
                    _logger?.LogInformation("Invalid float format");
                }
            }
        }
        catch(Exception ex)
        {
            _logger?.LogError(ex, "Error intercepting publish");
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _mqttServer?.StopAsync() ?? Task.CompletedTask;
    }

    public void Dispose()
    {
        _mqttServer?.Dispose();
    }
}