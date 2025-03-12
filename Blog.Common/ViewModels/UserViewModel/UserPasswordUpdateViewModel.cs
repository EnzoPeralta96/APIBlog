using System.ComponentModel.DataAnnotations;
namespace Common.ViewModels;
public class UserPasswordUpdateViewModel
{
    [Required(ErrorMessage = "El id de usuario es requerido")]
    public int Id { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(4, ErrorMessage = "La contraseña no puede ser menor a 4 cáracteres")]
    public string Password { get; set; }

    [Required(ErrorMessage = "La contraseña es requerida")]
    [MinLength(4, ErrorMessage = "La contraseña no puede ser menor a 4 cáracteres")]
    public string PasswordRepeat { get; set; }
}