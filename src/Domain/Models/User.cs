using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using TalentSchool.Domain.Enums;

namespace TalentSchool.Domain.Models;

public class User : IdentityUser
{
    [Required]
    public string? FullName { get; set; }
    
    [Required]
    public string? Code { get; set; }
    public UserType Type { get; set; }

    public ICollection<Progress>? Progresses { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
}