using Backend.WebApi.Domain.Models;

namespace Backend.WebApi.Repositories.Interfaces;
public delegate void SensorInfoPushedHandler(SensorInfo info);

public interface ISensorInfoRepository
{
    event SensorInfoPushedHandler OnSensorInfoPushed;
    SensorInfo GetLatestById(Guid id);

    IEnumerable<SensorInfo> GetLatestForAll();

    Task PushSensorInfo(SensorInfo info);
}