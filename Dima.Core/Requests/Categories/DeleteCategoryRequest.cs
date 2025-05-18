using System.ComponentModel.DataAnnotations;

namespace Dima.Core.Requests.Categories;

public class DeleteCategoryRequest : Request
{
    [Required(ErrorMessage = "O Id não pode ser vazio")]
    public long Id { get; set; }   
    
    [Required(ErrorMessage = "O Título da categoria não pode ser vazio.")]
    [MaxLength(80, ErrorMessage = "O Título não pode conter mais do que 80 caracteres.")]
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
}