using Microsoft.AspNetCore.Mvc;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using ApiEcommerce.Model.Dtos;
using ApiEcommerce.Models;

namespace ApiEcommerce.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CategoriesController : ControllerBase
  {
    private const string _modelStateCustomErrorKey = "CustomError";
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

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryDto createCategoryDto)
    {
      if (createCategoryDto == null)
      {
        return BadRequest(ModelState);
      }

      try
      {
        if (await _categoryRepository.CategoryExists(createCategoryDto.Name))
        {
          ModelState.AddModelError(_modelStateCustomErrorKey, "La categoría ya existe");
          return BadRequest(ModelState);
        }
        var category = _mapper.Map<Category>(createCategoryDto);
        var createdCategory = await _categoryRepository.CreateCategory(category);

        if (!createdCategory)
        {
          ModelState.AddModelError(_modelStateCustomErrorKey, $"Error al crear la categoria {category}");
          return StatusCode(500, ModelState);
        }
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
      }

      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor {ex.Message}");
      }
    }

    [HttpPatch("{id:int}", Name = "UpdateCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateCategory(int id, [FromBody] CreateCategoryDto updateCategoryDto)
    {
      if (updateCategoryDto == null || id <= 0)
      {
        return BadRequest(ModelState);
      }

      try
      {
        if (!await _categoryRepository.CategoryExists(id))
        {
          return NotFound($"No existe la categoria con el id {id}");
        }

        if (await _categoryRepository.CategoryExists(updateCategoryDto.Name))
        {
          ModelState.AddModelError(_modelStateCustomErrorKey, "La categoría ya existe");
          return BadRequest(ModelState);
        }
        var category = _mapper.Map<Category>(updateCategoryDto);
        category.Id = id;
        var updatedCategory = await _categoryRepository.UpdateCategory(category);

        if (!updatedCategory)
        {
          ModelState.AddModelError(_modelStateCustomErrorKey, $"Error al actualizar la categoria {category}");
          return StatusCode(500, ModelState);
        }
        return NoContent();
      }

      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor {ex.Message}");
      }
    }


    [HttpDelete("{id:int}", Name = "DeleteCategory")]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteCategory(int id)
    {
      try
      {
        if (!await _categoryRepository.CategoryExists(id))
        {
          return NotFound($"No existe la categoria con el id {id}");
        }

        var categoryToDelete = await _categoryRepository.GetCategory(id);

        if (categoryToDelete == null)
        {
          return NotFound($"La categoria no fue encontrada {categoryToDelete}");
        }
        if (!await _categoryRepository.DeleteCategory(categoryToDelete))
        {
          ModelState.AddModelError(_modelStateCustomErrorKey, $"Error al borrar la categoria {categoryToDelete}");
          return StatusCode(500, ModelState);
        }
        return NoContent();
      }

      catch (Exception ex)
      {
        return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno del servidor {ex.Message}");
      }
    }
  }
}