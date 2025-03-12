using System.ComponentModel.DataAnnotations;
namespace Common.ViewModels;

public class PostUpdateViewModel
{
    [Required(ErrorMessage = "El PostId es requerido")]
    public int PostId { get; set; }

    [Required(ErrorMessage = "El titulo del post es requerido")]
    [MaxLength(50)]
    public string Title { get; set; }

    [Required(ErrorMessage = "El contenido del post es requerido")]
    [MaxLength(1000)]
    public string Content { get; set; }

}