using System.ComponentModel.DataAnnotations;

namespace TalentSchool.Domain.Enums;

public enum UserType
{
    [Display(Name = "Administrador")]
    Admin,
    [Display(Name = "Empleado")]
    Employee,
}