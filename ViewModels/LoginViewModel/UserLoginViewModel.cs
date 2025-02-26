using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace APIBlog.ViewModels;
public class UserLoginViewModel
{
    [MaxLength(25, ErrorMessage = "El nombre no puede ser mayor a 25 cáracteres")]
    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    public string Name { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(4,ErrorMessage = "La contraseña no puede ser menor a 4 cáracteres")]
    public string Password { get; set; }
}