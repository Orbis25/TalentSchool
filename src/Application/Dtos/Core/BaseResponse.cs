namespace TalentSchool.Application.Dtos.Core;

public class BaseResponse<T>
{
    public string? Error { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public bool Success => string.IsNullOrEmpty(Error);
}