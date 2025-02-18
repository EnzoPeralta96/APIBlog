using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APIBlog.ViewModels;

public class PostRequestViewModel
{
    [Required(ErrorMessage = "El titulo del post es requerido")]
    [MaxLength(50)]
    public string Title { get; set; }
    [Required(ErrorMessage = "El contenido del post es requerido")]
    [MaxLength(1000)]
    public string Content { get; set; }
    
}