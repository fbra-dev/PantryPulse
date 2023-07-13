using Backend.WebApi.Domain.Models;
using Backend.WebApi.Infrastructure.Persistence;
using Backend.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.WebApi.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly SensorServiceContext _context;

    public ProductRepository(SensorServiceContext context)
    {
        this._context = context;
    }

    /// <inheritdoc />
    public async Task<Product?> GetByIDAsync(Guid id) =>
        await this._context.Products.FirstOrDefaultAsync(product => product.ID == id);

    /// <inheritdoc />
    public async Task<IEnumerable<Product>> GetAllAsync() => await this._context.Products.ToListAsync();

    /// <inheritdoc />
    public async Task AddAsync(Product product)
    {
        Product? existingProduct = await this.GetByIDAsync(product.ID);

        if (existingProduct != null)
        {
            throw new ArgumentException($"A product with id '{product.ID}' has already been added!", nameof(product));
        }

        await this._context.Products.AddAsync(product);
        await this._context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task RenameAsync(Guid id, string newName)
    {
        Product? product = await this.GetByIDAsync(id);

        if (product == null)
        {
            throw new ArgumentException($"A product with id '{id}' does not exist!", nameof(id));
        }

        product.Name = newName;

        await this._context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task SetRegularWeightAsync(Guid id, float regularWeight)
    {
        Product? product = await this.GetByIDAsync(id);

        if (product == null)
        {
            throw new ArgumentException($"A product with id '{id}' does not exist!", nameof(id));
        }

        product.RegularWeight = regularWeight;

        await this._context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task AddAutomatedActionAsync(Guid id, AutomatedAction action)
    {
        Product? product = await this.GetByIDAsync(id);

        if (product == null)
        {
            throw new ArgumentException($"A product with id '{id}' does not exist!", nameof(id));
        }

        List<AutomatedAction> actions = product.AutomatedActions?.ToList() ?? new List<AutomatedAction>();

        actions.Add(action);

        product.AutomatedActions = actions;

        await this._context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task RemoveAutomatedActionAsync(Guid id, Guid actionID)
    {
        Product? product = await this.GetByIDAsync(id);

        if (product == null)
        {
            throw new ArgumentException($"A product with id '{id}' does not exist!", nameof(id));
        }

        List<AutomatedAction> actions = product.AutomatedActions?.ToList() ?? new List<AutomatedAction>();

        actions.RemoveAll(action => action.ID == actionID);

        product.AutomatedActions = actions;

        await this._context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task UpdateAsync(Product product)
    {
        Product? existingProduct = await this.GetByIDAsync(product.ID);

        if (existingProduct == null)
        {
            throw new ArgumentException($"A product with id '{product.ID}' does not exist!", nameof(product));
        }

        // Copy values of 'product' to 'existingProduct'
        _context.Entry(existingProduct).CurrentValues.SetValues(product);

        await this._context.SaveChangesAsync();
    }

}