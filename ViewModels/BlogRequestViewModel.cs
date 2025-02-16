
using System.ComponentModel.DataAnnotations;
using APIBlog.Models;
namespace APIBlog.ViewModels;
public class BlogRequestViewModel
{
    [Required(ErrorMessage = "El titulo del blog es requerido")]
    [MaxLength(50)]
    public string Name { get; set; }

    [Required(ErrorMessage = "La descripción del blog es requerida")]
    [MaxLength(200)]
    public string Description { get; set; }
}