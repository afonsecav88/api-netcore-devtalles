using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Model.Dtos
{
  public class CreateCategoryDto
  {
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [MaxLength(50, ErrorMessage = "El nombre no debe de tener mas de 50 caracteres")]
    [MinLength(3, ErrorMessage = "El nombre no debe de tener menos de 3 caracteres")]
    public string Name { get; set; } = string.Empty;
  }
}