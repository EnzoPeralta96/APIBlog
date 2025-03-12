using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Timers;

namespace Common.ViewModels;
public class UserUpdateViewModel
{
    [Required(ErrorMessage = "El id de usuario es requerido")]
    public int Id{get; set;}

    [MaxLength(25,ErrorMessage = "El nombre no puede ser mayor a 25 cáracteres")]
    [Required(ErrorMessage = "El nombre de usuario es requerido")]
    public string Name{get; set;}

}