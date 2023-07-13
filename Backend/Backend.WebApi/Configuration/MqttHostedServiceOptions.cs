namespace Backend.WebApi.Configuration;

public class MqttHostedServiceOptions
{
    public short ServerPort { get; set; }
    
    public string Topic { get; set; }
}