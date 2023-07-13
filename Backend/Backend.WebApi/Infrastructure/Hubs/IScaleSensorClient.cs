using Backend.WebApi.Controllers.Models;

namespace Backend.WebApi.Infrastructure.Hubs;

public interface IScaleSensorClient
{
    Task Initialize(IEnumerable<SensorDto> sensorInfos);

    Task ReceiveSensorInfo(SensorInfoDto sensorInfo);

    Task RenameSensor(Guid id, string newName);
}