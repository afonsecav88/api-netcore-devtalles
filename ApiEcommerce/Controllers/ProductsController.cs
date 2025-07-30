using ApiEcommerce.Model;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController : ControllerBase
  {
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
    public ProductsController(IProductRepository productRepository, IMapper mapper)
    {
      _productRepository = productRepository;
      _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetProducts()
    {
      try
      {
        var products = await _productRepository.GetProducts();
        var productsDto = new List<ProductDto>();
        foreach (var product in products)
        {
          productsDto.Add(_mapper.Map<ProductDto>(product));
        }
        return Ok(productsDto);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError,
       $"Error interno del servidor {ex.Message}");
      }
    }
  }
}
