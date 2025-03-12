using System.ComponentModel.DataAnnotations;

namespace Common.ViewModels;

public class CommentUpdateViewModel
{

    [Required(ErrorMessage = "El Id del comentario es requerido")]
    public int Id { get; set; }

    [MaxLength(1000)]
    [Required(ErrorMessage = "El comentario es requerido")]
    public string Comentary { get; set; }
}