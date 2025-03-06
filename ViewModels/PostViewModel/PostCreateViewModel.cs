using System.ComponentModel.DataAnnotations;


namespace APIBlog.ViewModels;

public class PostCreateViewModel
{
    [Required(ErrorMessage = "El BlogId es requerido")]
    public int BlogId {get; set;}
    
    [Required(ErrorMessage = "El titulo del post es requerido")]
    [MaxLength(50)]
    public string Title { get; set; }
    [Required(ErrorMessage = "El contenido del post es requerido")]
    [MaxLength(1000)]
    public string Content { get; set; }

    [Required(ErrorMessage = "El Id del creador del Post es requerido")]
    public int OwnerPostId {get; set;}
}