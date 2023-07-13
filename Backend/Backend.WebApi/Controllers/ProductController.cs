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
public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IProductRepository productRepository, IMapper mapper, ILogger<ProductController> logger)
    {
        this._productRepository = productRepository;
        this._mapper = mapper;
        this._logger = logger;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByID(Guid id)
    {
        Product? product = await this._productRepository.GetByIDAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        ProductDto mappedProduct = this._mapper.Map<ProductDto>(product);

        return Ok(mappedProduct);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
    public async Task<IActionResult> GetAll()
    {
        IEnumerable<Product> products = await this._productRepository.GetAllAsync();
        IEnumerable<ProductDto> mappedProducts = this._mapper.Map<IEnumerable<ProductDto>>(products);

        return Ok(mappedProducts);
    }

    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] ProductDto product)
    {
        //TODO: FBra --> make good
        product.ID = Guid.NewGuid();
        Product mappedProduct = this._mapper.Map<Product>(product);
        
        try
        {
            await this._productRepository.AddAsync(mappedProduct);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPatch()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromBody] ProductDto product)
    {
        try
        {
            Product productToUpdate = _mapper.Map<Product>(product);
            await this._productRepository.UpdateAsync(productToUpdate);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("setRegularWeight/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SetRegularWeight(Guid id, [FromBody] float regularWeight)
    {
        try
        {
            await this._productRepository.SetRegularWeightAsync(id, regularWeight);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost("addAutomatedAction/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddAutomatedAction(Guid id, [FromBody] AutomatedActionDto action)
    {
        AutomatedAction? mappedAction = this._mapper.Map<AutomatedAction>(action);
        
        try
        {
            await this._productRepository.AddAutomatedActionAsync(id, mappedAction);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
    
    [HttpPost("removeAutomatedAction/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemoveAutomatedAction(Guid id, Guid actionID)
    {
        try
        {
            await this._productRepository.RemoveAutomatedActionAsync(id, actionID);

            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}