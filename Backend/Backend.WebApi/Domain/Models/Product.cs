namespace Backend.WebApi.Domain.Models;

public record Product : IIdentifiable
{
    public Guid ID { get; init; }
    
    public string Name { get; set; }

    public float RegularWeight { get; set; }
    
    public IEnumerable<AutomatedAction>? AutomatedActions { get; set; }
}