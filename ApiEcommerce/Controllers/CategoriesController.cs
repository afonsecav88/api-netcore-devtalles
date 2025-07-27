using Microsoft.AspNetCore.Mvc;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using ApiEcommerce.Model.Dtos;

namespace ApiEcommerce.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase
  {
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
    {
      _categoryRepository = categoryRepository;
      _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategories()
    {
      try
      {
        var categories = await _categoryRepository.GetCategories();
        var categoriesDto = new List<CategoryDto>();
        foreach (var category in categories)
        {
          categoriesDto.Add(_mapper.Map<CategoryDto>(category));
        }

        return Ok(categoriesDto);
      }
      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError,
       $"Error interno del servidor {ex.Message}");
      }
    }

    [HttpGet("{id:int}", Name = "GetCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategory(int id)
    {
      try
      {
        var category = await _categoryRepository.GetCategory(id);
        if (category == null)
        {
          return NotFound($"La categoria con el id {id} no existe");
        }

        var categoriesDto = _mapper.Map<CategoryDto>(category);

        return Ok(categoriesDto);
      }

      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor {ex.Message}");
      }
    }
  }
}