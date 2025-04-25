namespace TalentSchool.Application.Core.Context;

public interface IContext
{
    public string? Email { get; set; }
    public string? UserId { get; set; }
}