using AutoMapper;
using Backend.WebApi.Controllers.Models;
using Backend.WebApi.Domain.Models;
using Backend.WebApi.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.WebApi.Controllers;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
[ApiController]
public class SensorController : Controller
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<SensorController> _logger;

    public SensorController(ISensorRepository sensorRepository, IMapper mapper, ILogger<SensorController> logger)
    {
        this._sensorRepository = sensorRepository;
        this._mapper = mapper;
        this._logger = logger;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SensorDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByID(Guid id)
    {
        Sensor? sensor = await this._sensorRepository.GetByIDAsync(id);

        if (sensor == null)
        {
            return NotFound();
        }

        SensorDto mappedSensor = this._mapper.Map<SensorDto>(sensor);

        return Ok(mappedSensor);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SensorDto>))]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Sensor> sensors = await this._sensorRepository.GetAllAsync();
        IEnumerable<SensorDto> mappedSensors = this._mapper.Map<IEnumerable<SensorDto>>(sensors);

        return Ok(mappedSensors);
    }

    [HttpPost("add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] SensorDto sensor)
    {
        Sensor? mappedSensor = this._mapper.Map<Sensor>(sensor);
        
        try
        {
            await this._sensorRepository.AddAsync(mappedSensor);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("setProduct/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetProduct(Guid id, [FromBody] ProductDto product)
    {
        Product? mappedProduct = this._mapper.Map<Product>(product);
        
        try
        {
            await this._sensorRepository.SetProductAsync(id, mappedProduct);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("rename/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Rename(Guid id, [FromBody] string name)
    {
        try
        {
            await this._sensorRepository.RenameAsync(id, name);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}