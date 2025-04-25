using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoundationKit.Domain.Models;

namespace TalentSchool.Domain.Models;

public class Module : BaseBasicModel
{
    [Display(Name = "Titulo")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [MinLength(3,ErrorMessage = "Ingrese al menos 3 caracteres")]
    public string? Title { get; set; }
    
    [Display(Name = "Categoria")]
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [MaxLength(100,ErrorMessage = "El campo {0} no puede tener más de 100 caracteres")]
    public string? Category { get; set; }
    public List<StaticContent>? Contents { get; set; }
    public List<Progress>? Progresses { get; set; }
}