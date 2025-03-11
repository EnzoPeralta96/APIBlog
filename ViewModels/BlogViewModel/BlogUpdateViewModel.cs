
using System.ComponentModel.DataAnnotations;
namespace APIBlog.ViewModels;
public class BlogUpdateViewModel
{
    [Required(ErrorMessage = "El Id del blog es requerido")]
    public int IdBlog { get; set; }

    [Required(ErrorMessage = "El titulo del blog es requerido")]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required(ErrorMessage = "La descripción del blog es requerida")]
    [MaxLength(200)]
    public string Description { get; set; }
}