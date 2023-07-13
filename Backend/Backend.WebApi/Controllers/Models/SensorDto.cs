namespace Backend.WebApi.Controllers.Models;

public class SensorDto
{
    public Guid ID { get; set; }

    public string Name { get; set; }
    
    public ProductDto? Product { get; set; }
}