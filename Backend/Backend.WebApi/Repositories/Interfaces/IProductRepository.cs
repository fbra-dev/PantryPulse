using Backend.WebApi.Controllers.Models;
using Backend.WebApi.Domain.Models;

namespace Backend.WebApi.Repositories.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIDAsync(Guid id);

    Task<IEnumerable<Product>> GetAllAsync();

    Task AddAsync(Product product);

    Task RenameAsync(Guid id, string newName);

    Task SetRegularWeightAsync(Guid id, float regularWeight);

    Task AddAutomatedActionAsync(Guid id, AutomatedAction action);

    Task RemoveAutomatedActionAsync(Guid id, Guid actionID);
    Task UpdateAsync(Product product);
}