namespace Backend.WebApi.Domain.Models;

public interface IIdentifiable
{
    Guid ID { get; }
}