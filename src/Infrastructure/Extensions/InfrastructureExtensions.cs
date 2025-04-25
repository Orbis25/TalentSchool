using FoundationKit.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TalentSchool.Application.Core.Context;
using TalentSchool.Application.EF.Repositories;
using TalentSchool.Application.Services;
using TalentSchool.Application.Services.UsersService;
using TalentSchool.Domain.Models;
using TalentSchool.Infrastructure.EF;
using TalentSchool.Infrastructure.Persistence;
using TalentSchool.Infrastructure.Seeds;

namespace TalentSchool.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRazorPages().AddRazorRuntimeCompilation();
;        services.AddHttpContextAccessor();
        services.AddFoundationKitIdentity<User, ApplicationDbContext>();
        services.ConfigureDbContext(configuration)
            .AddIdentityConfig();
        
        services.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Identity/Account/Login";
            options.AccessDeniedPath = "/Identity/Account/Login";
        });

        services.AddScoped<IContext, Context>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IModuleRepository, ModuleRepository>();
        services.AddScoped<IModuleService, ModuleService>();
        services.AddScoped<IProgressRepository, ProgressRepository>();
        services.AddScoped<IProgressService, ProgressService>();

        return services;
    }

    private static IServiceCollection ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(option =>
        {
            option.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
        });

        return services;
    }

    private static IServiceCollection AddIdentityConfig(this IServiceCollection services)
    {
        services.Configure<IdentityOptions>(opt =>
        {
            opt.Password.RequireDigit = false;
            opt.Password.RequireLowercase = false;
            opt.Password.RequireUppercase = false;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 6;
            opt.User.RequireUniqueEmail = true;
        });

        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.ApplySeedConfiguration();
        app.UseAuthentication();
        app.UseAuthorization();
        return app;
    }
}