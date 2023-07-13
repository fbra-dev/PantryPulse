using TestMqttClient;

var mqttClientService = new MqttService();
await mqttClientService.ConnectAsync();

string? line;
while (!string.IsNullOrEmpty(line = Console.ReadLine()))
{
    await mqttClientService.PublishAsync(line);
}