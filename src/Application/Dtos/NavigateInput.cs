namespace TalentSchool.Application.Dtos;

public class NavigateInput
{
    public Guid ModuleId { get; set; }
    public int CurrentIndex { get; set; }
    public string? Direction { get; set; }
}