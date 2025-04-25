using System.Security.Claims;

namespace TalentSchool.Application.Core.Context;

public class Context : IContext
{
    public string? Email { get; set; }
    public string? UserId { get; set; }

    public Context(IHttpContextAccessor httpContextAccessor)
    {
        try
        {
            var user = httpContextAccessor.HttpContext?.User;

            if (user?.Identity is not { IsAuthenticated: true }) return;
            
            Email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            UserId = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}