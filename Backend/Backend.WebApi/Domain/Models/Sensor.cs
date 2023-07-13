namespace Backend.WebApi.Domain.Models;

public record Sensor : IIdentifiable
{
    public Guid ID { get; init; }

    public string Name { get; set; }

    public Product? Product { get; set; }
}