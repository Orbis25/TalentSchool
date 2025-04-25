using FoundationKit.Domain.Models;
using TalentSchool.Domain.Enums;

namespace TalentSchool.Domain.Models;

public class Progress : BaseBasicModel
{
    public string? UserId { get; set; }
    public User? User { get; set; }
    
    public Guid ModuleId { get; set; }
    public Module? Module { get; set; }
    
    public DateTime? CompletedAt { get; set; }
    
    public int? CurrentContentIndex { get; set; }
    
    public ProgressStatus Status { get; set; }
}