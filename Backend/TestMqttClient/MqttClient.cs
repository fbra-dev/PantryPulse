using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;

namespace TestMqttClient;

public class MqttService
{
    private const string SERVER_IP = "localhost";
    private const int SERVER_PORT = 1883;
    private const string CLIENT_ID = "mqtt_consumer";
    private const string TOPIC = "esp32/weight";
    
    private readonly IMqttClient _mqttClient;
    private readonly MqttClientOptions _clientOptions;

    public MqttService()
    {
        _mqttClient = new MqttFactory().CreateMqttClient();

        _clientOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(SERVER_IP, SERVER_PORT)
            .WithClientId(CLIENT_ID)
            .Build();
            
        _mqttClient.ConnectedAsync += HandleConnectedAsync;
        _mqttClient.DisconnectedAsync += HandleDisconnectedAsync;
        _mqttClient.ApplicationMessageReceivedAsync += HandleMessageReceivedAsync;
    }

    public async Task ConnectAsync()
    {
        try
        {
            await _mqttClient.ConnectAsync(_clientOptions, CancellationToken.None);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connecting to MQTT broker failed. Exception: {ex.Message}");
        }
    }

    public async Task PublishAsync(string? message)
    {
        MqttApplicationMessage mqttMessage = new()
        {
            PayloadSegment = Encoding.UTF8.GetBytes(message),
            Topic = TOPIC
        };

        await _mqttClient.PublishAsync(mqttMessage);
    }

    private async Task HandleConnectedAsync(MqttClientConnectedEventArgs args)
    {
        Console.WriteLine("Connected to MQTT broker.");

        MqttTopicFilter topicFilter = new MqttTopicFilterBuilder()
            .WithTopic(TOPIC)
            .Build();

        await _mqttClient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder()
            .WithTopicFilter(topicFilter)
            .Build());
    }

    private async Task HandleDisconnectedAsync(MqttClientDisconnectedEventArgs args)
    {
        Console.WriteLine($"Disconnected from MQTT broker. Reason: {args.Reason}");

        await Task.Delay(TimeSpan.FromSeconds(5));

        try
        {
            await _mqttClient.ConnectAsync(_clientOptions, CancellationToken.None);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Reconnecting to MQTT broker failed. Exception: {ex.Message}");
        }
    }

    private Task HandleMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args)
    {
        string message = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

        Console.WriteLine($"Received message on topic '{args.ApplicationMessage.Topic}': {message}");

        return Task.CompletedTask;
    }
}