namespace Backend.WebApi.Domain.Models;

public abstract record AutomatedAction
{
    public Guid ID { get; init; }
}