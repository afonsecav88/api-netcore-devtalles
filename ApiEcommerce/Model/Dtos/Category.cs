using System.ComponentModel.DataAnnotations;

public class Category
{
  [Key]
  public int Int { get; set; }
  [Required]
  public string Name { get; set; } = string.Empty;
  [Required]
  public DateTime CreationDate { get; set; }
}