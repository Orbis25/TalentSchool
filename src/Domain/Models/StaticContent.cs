using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FoundationKit.Domain.Models;

namespace TalentSchool.Domain.Models;

public class StaticContent : BaseBasicModel
{
         
    [Required(ErrorMessage = "El índice es obligatorio")]
    [Display(Name = "Índice")]
    public int Index { get; set; }
    
    [Required(ErrorMessage = "El contenido es obligatorio")]
    [Display(Name = "Contenido")]
    public string? Content { get; set; }
    public Guid ModuleId { get; set; }
    public Module? Module { get; set; }
}