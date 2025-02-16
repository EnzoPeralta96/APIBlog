using System.ComponentModel.DataAnnotations;

namespace APIBlog.ViewModels;

public class CreatePostViewModels
{
    [Required(ErrorMessage = "El titulo del post es requerido")]
    [MaxLength(50)]
    public string Title { get; set; }
    [Required(ErrorMessage = "El contenido del post es requerido")]
    [MaxLength(1000)]
    public string Content { get; set; }

    [Required(ErrorMessage = "El id del blog al que pertenecerá el post es requerido")]
    public int BlogId { get; set; }
}