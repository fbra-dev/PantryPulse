using Backend.WebApi.Domain.Models;

namespace Backend.WebApi.Repositories.Interfaces;

public interface ISensorRepository
{
    Task<Sensor?> GetByIDAsync(Guid id);

    Task<IEnumerable<Sensor>> GetAllAsync();

    Task AddAsync(Sensor sensor);

    Task SetProductAsync(Guid id, Product product);

    Task RenameAsync(Guid id, string newName);
    Task UpdateAsync(Sensor sensor);
}