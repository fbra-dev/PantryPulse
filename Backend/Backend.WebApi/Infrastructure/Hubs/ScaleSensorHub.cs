using AutoMapper;
using Backend.WebApi.Controllers.Models;
using Backend.WebApi.Domain.Models;
using Backend.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Backend.WebApi.Infrastructure.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ScaleSensorHub : Hub<IScaleSensorClient>
    {
        private readonly IProductRepository _productRepository;
        private readonly ISensorRepository _sensorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ScaleSensorHub> _logger;

        public ScaleSensorHub(ISensorRepository sensorRepository,IProductRepository productRepository,IMapper mapper, ILogger<ScaleSensorHub> logger)
        {
            _productRepository = productRepository;
            _sensorRepository = sensorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task SendSensorInfoAsync(SensorInfo sensorInfo)
        {
            SensorInfoDto? sensorInfoDto = _mapper.Map<SensorInfoDto>(sensorInfo);

            await Clients.All.ReceiveSensorInfo(sensorInfoDto);
            _logger.LogInformation("Sensor info sent to all clients");
        }

        public async Task UpdateSensor(SensorDto sensorDto)
        {
            Sensor sensor = _mapper.Map<Sensor>(sensorDto);
            
            if (sensor.Product != null && sensor.Product.ID != Guid.Empty)
            {
                sensor.Product = await _productRepository.GetByIDAsync(sensor.Product.ID);
            }
            await _sensorRepository.UpdateAsync(sensor);
        }
        
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Client with id \'{ContextConnectionId}\' disconnected", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }

        public override async Task OnConnectedAsync()
        {
            var values = _mapper.Map<IEnumerable<SensorDto>>(await _sensorRepository.GetAllAsync());

            await Clients.Caller.Initialize(values);
            _logger.LogInformation("Client with id \'{ContextConnectionId}\' connected and initialized", Context.ConnectionId);
            await base.OnConnectedAsync();
        }
    }
}