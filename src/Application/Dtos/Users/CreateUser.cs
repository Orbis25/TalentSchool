
using System.ComponentModel.DataAnnotations;

namespace TalentSchool.Application.Dtos.Users;

public class CreateUser
{
    [Display(Name = "Correo")]
    [Required(ErrorMessage = "Campo {0} requerido")]
    [EmailAddress(ErrorMessage = "El campo {0} no es un correo valido")]
    public string? Email { get; set; }
    
    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "Campo {0} requerido")]
    [MaxLength(100, ErrorMessage = "La longitud maxima de 100 caracteres")]
    public string? FullName { get; set; }
    
    [MinLength(6, ErrorMessage = "La longitud minima es de 6 caracteres")]
    [Display(Name = "Contraseña")]
    [Required(ErrorMessage = "Campo {0} requerido")]
    public string? Password { get; set; }
    
    [MinLength(3, ErrorMessage = "La longitud minima es de 3 caracteres")]
    [Display(Name = "Codigo")]
    [Required(ErrorMessage = "Campo {0} requerido")]
    public string? Code { get; set; }
}