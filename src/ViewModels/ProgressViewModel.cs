using System.ComponentModel.DataAnnotations;
using TalentSchool.Domain.Enums;
using TalentSchool.Domain.Models;

namespace TalentSchool.ViewModels;

public class ProgressViewModel
{
    public string UserId { get; set; }
    public Guid ModuleId { get; set; }
    public string ModuleTitle { get; set; }
    public string ModuleCategory { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? CurrentContentIndex { get; set; }
    public ProgressStatus Status { get; set; }
    public int TotalContents { get; set; }

    public int ProgressPercentage =>
        CurrentContentIndex.HasValue && TotalContents > 0
            ? (int)Math.Round((double)CurrentContentIndex.Value / TotalContents * 100)
            : 0;
}

public class UserProgressViewModel
{
    public User User { get; set; }
    public List<ProgressViewModel> Progresses { get; set; }
}

public class AssignProgressViewModel
{
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Usuario")]

    public string UserId { get; set; }
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Modulo")]

    public Guid ModuleId { get; set; }
    [Required(ErrorMessage = "El campo {0} es requerido")]
    [Display(Name = "Estado")]

    public ProgressStatus Status { get; set; } = ProgressStatus.New;
}

public class DeleteProgressViewModel
{
    public string UserId { get; set; }
    public Guid ModuleId { get; set; }
    public string UserName { get; set; }
    public string ModuleTitle { get; set; }
    public ProgressStatus Status { get; set; }
    public DateTime? CompletedAt { get; set; }
    public int? CurrentContentIndex { get; set; }
}

public class TakeModuleViewModel
{
    public string? UserId { get; set; }
    public Guid ModuleId { get; set; }
    public string? ModuleTitle { get; set; }
    public int CurrentContentIndex { get; set; }
    public StaticContent? CurrentContent { get; set; }
    public int TotalContents { get; set; }
    public ProgressStatus Status { get; set; }
    public DateTime? CompletedAt { get; set; }
    public bool HasPrevious { get; set; }
    public bool HasNext { get; set; }
    public bool IsLastContent { get; set; }

    public int ProgressPercentage =>
        TotalContents > 0
            ? (int)Math.Round((double)CurrentContentIndex / TotalContents * 100)
            : 0;
}