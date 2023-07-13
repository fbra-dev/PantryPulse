namespace Backend.WebApi.Controllers.Models;

public class ProductDto
{
    public Guid? ID { get; set; }

    public string Name { get; set; }

    public float RegularWeight { get; set; }

    public IEnumerable<AutomatedActionDto>? AutomatedActions { get; set; }
}