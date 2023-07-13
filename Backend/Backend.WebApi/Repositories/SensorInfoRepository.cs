using System.Collections.Concurrent;
using Backend.WebApi.Domain.Models;
using Backend.WebApi.Repositories.Interfaces;

namespace Backend.WebApi.Repositories;

public class SensorInfoRepository : ISensorInfoRepository
{
    private readonly ConcurrentDictionary<Guid, SensorInfo> _latestSensorInfo = new();
    private ILogger<SensorInfoRepository> _logger;

    public event SensorInfoPushedHandler? OnSensorInfoPushed;
    
    public SensorInfoRepository(ILogger<SensorInfoRepository> logger)
    {
        this._logger = logger;
    }

    public SensorInfo GetLatestById(Guid id)
    {
        return this._latestSensorInfo[id];
    }

    public IEnumerable<SensorInfo> GetLatestForAll()
    {
        return this._latestSensorInfo.Values;
    }

    public Task PushSensorInfo(SensorInfo info)
    {
        this._latestSensorInfo[info.ID] = info;

        OnSensorInfoPushed?.Invoke(info);
        
        return Task.CompletedTask;
    }
}