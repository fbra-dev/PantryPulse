namespace Backend.WebApi.Domain.Models;

public record CallWebServiceAction : AutomatedAction
{
    public string Url { get; set; }

    public Dictionary<string, string> Parameters { get; set; }
}