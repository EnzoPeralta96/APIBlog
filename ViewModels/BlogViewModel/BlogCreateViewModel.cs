
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
namespace APIBlog.ViewModels;

public class BlogCreateViewModel
{
    [Required(ErrorMessage = "El Id del usuario propietario es requerido")]
    public int OwnerBlogId {get; set;}

    [Required(ErrorMessage = "El titulo del blog es requerido")]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required(ErrorMessage = "La descripción del blog es requerida")]
    [MaxLength(200)]
    public string Description { get; set; }
}
