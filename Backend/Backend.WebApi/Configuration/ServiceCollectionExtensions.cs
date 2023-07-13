namespace Backend.WebApi.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMqttHostedServiceOptions(this IServiceCollection services,
        IConfiguration config) =>
        services.Configure<MqttHostedServiceOptions>(
            config.GetSection(nameof(MqttHostedServiceOptions)));
    
    public static IServiceCollection AddConnectionStrings(this IServiceCollection services,
        IConfiguration config) =>
        services.Configure<ConnectionStrings>(
            config.GetSection(nameof(ConnectionStrings)));
}