namespace Backend.WebApi.Controllers.Models;

public class CallWebServiceActionDto : AutomatedActionDto
{
    public string Url { get; set; }
    
    public Dictionary<string, string> Parameters { get; set; }
}