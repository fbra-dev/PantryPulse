using Backend.WebApi.Domain.Models;
using Backend.WebApi.Infrastructure.Persistence;
using Backend.WebApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.WebApi.Repositories;

public class SensorRepository : ISensorRepository
{
    private readonly SensorServiceContext _context;

    public SensorRepository(SensorServiceContext context)
    {
        this._context = context;
    }

    /// <inheritdoc />
    public async Task<Sensor?> GetByIDAsync(Guid id) =>
        await this._context.Sensors.FirstOrDefaultAsync(sensor => sensor.ID == id);

    /// <inheritdoc />
    public async Task<IEnumerable<Sensor>> GetAllAsync() => await this._context.Sensors.ToListAsync();

    /// <inheritdoc />
    public async Task AddAsync(Sensor sensor)
    {
        Sensor? existingSensor = await this.GetByIDAsync(sensor.ID);

        if (existingSensor != null)
        {
            throw new ArgumentException($"A sensor with id '{sensor.ID}' has already been added!", nameof(sensor));
        }

        await this._context.Sensors.AddAsync(sensor);
        await this._context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task SetProductAsync(Guid id, Product product)
    {
        Sensor? sensor = await this.GetByIDAsync(id);

        if (sensor == null)
        {
            throw new ArgumentException($"A sensor with id '{id}' does not exist!", nameof(id));
        }

        sensor.Product = product;

        await this._context.SaveChangesAsync();
    }

    /// <inheritdoc />
    public async Task RenameAsync(Guid id, string newName)
    {
        Sensor? sensor = await this.GetByIDAsync(id);

        if (sensor == null)
        {
            throw new ArgumentException($"A sensor with id '{id}' does not exist!", nameof(id));
        }

        sensor.Name = newName;

        await this._context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Sensor sensor)
    {
        this._context.Update(sensor);
        await this._context.SaveChangesAsync();
    }
}