using System.ComponentModel.DataAnnotations;

namespace APIBlog.ViewModels;

public class CommentCreateViewModel
{

    [MaxLength(1000)]
    [Required(ErrorMessage = "El comentario es requerido")]
    public string Comentary { get; set; }

    [Required(ErrorMessage = "El Id de usuario es requerido")]
    public int UserId { get; set; }

    [Required (ErrorMessage = "El Id del Post es requerido")]
    public int PostId { get; set; }

}