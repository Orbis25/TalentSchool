using System.Linq.Expressions;
using FoundationKit.Domain.Dtos.Paginations;
using Foundationkit.Extensions.Enumerations;
using Foundationkit.Extensions.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TalentSchool.Application.Core.Context;
using TalentSchool.Application.Dtos.Core;
using TalentSchool.Application.Dtos.Users;
using TalentSchool.Application.EF.Repositories;
using TalentSchool.Domain.Enums;
using TalentSchool.Domain.Models;

namespace TalentSchool.Application.Services.UsersService;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IUserRepository _userRepository;
    private readonly IContext _context;

    public UserService(UserManager<User> userManager,
        SignInManager<User> signInManager,
        RoleManager<IdentityRole> roleManager,
        IUserRepository userRepository,
        IContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _userRepository = userRepository;
        _context = context;
    }

    public async Task<BaseResponse<bool>> RegisterUserAsync(CreateUser user, bool system = false)
    {
        try
        {
            var data = new User
            {
                Email = user.Email,
                NormalizedEmail = user.Email.ToUpper(),
                UserName = user.Email,
                NormalizedUserName = user.Email.ToUpper(),
                FullName = user.FullName,
                Type = system ? UserType.Admin : UserType.Employee,
                Code = user.Code,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _userManager.CreateAsync(data, user.Password!);

            if (!result.Succeeded)
                return new()
                {
                    Data = result.Succeeded,
                    Error = result.Errors.FirstOrDefault()?.Description
                };


            var r = await _userManager.AddToRoleAsync(data, data.Type.GetAttribute());
            return new BaseResponse<bool>()
            {
                Data = true,
                Message = "User registered successfully."
            };
        }
        catch (Exception e)
        {
            return new()
            {
                Error = e.Message
            };
        }
    }


    public async Task<BaseResponse<bool>> SignInUserAsync(string email, string password)
    {
        try
        {
            var result =
                await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);
            return new();
        }
        catch (Exception e)
        {
            return new()
            {
                Error = e.Message
            };
        }
    }

    public async Task<User?> Get(Expression<Func<User, bool>> predicate)
    {
        return await _userManager.Users.FirstOrDefaultAsync(predicate).ConfigureAwait(false);
    }

    public async Task<PaginationResult<User>> GetAllAsync(Paginate paginate,
        CancellationToken cancellationToken = default)
    {
        var items = _userManager.Users.Where(x => x.Id != _context.UserId)
            .AsQueryable();

        if (!string.IsNullOrEmpty(paginate.Query))
        {
            items = items.Where(x =>
                x.Email!.Contains(paginate.Query) || x.FullName!.Contains(paginate.Query));
        }

        paginate.NoPaginate = true;
        var result = await items.PaginateAsync(paginate, cancellationToken);

        result.Query = paginate.Query;
        return result!;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var existRole = _roleManager.Roles.Any();
            var existUser = _userManager.Users.Any();

            if (existRole && existUser)
                return;


            _ = await _roleManager.CreateAsync(new() { Name = UserType.Admin.GetAttribute() });
            _ = await _roleManager.CreateAsync(new() { Name = UserType.Employee.GetAttribute() });

            var admin = new CreateUser
            {
                Email = "admin@admin.com",
                FullName = "Admin",
                Password = "admin123",
                Code = "amd"
            };
            _ = await RegisterUserAsync(admin, true);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<bool> EditAsync(User user, CancellationToken cancellationToken = default)
    {
        try
        {
            var data = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);

            if (data == null)
                return false;

            data.FullName = user.FullName;
            data.Email = user.Email;
            data.NormalizedEmail = user.Email!.ToUpper();
            data.UserName = user.Email;
            data.NormalizedUserName = user.Email.ToUpper();
            data.Code = user.Code;

            var result = await _userManager.UpdateAsync(data);

            return result.Succeeded;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    
    public async Task<bool> SoftRemoveAsync(string id, CancellationToken cancellationToken = default)
    {
        try
        {
            var data = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (data == null)
                return false;

            var deleted = Guid.NewGuid();

            data.Email = deleted+"@deleted.com";
            data.NormalizedEmail = deleted+"@deleted.com";
            data.UserName = deleted+"@deleted.com";
            data.NormalizedUserName = deleted+"@deleted.com";
            data.IsDeleted = true;

            var result = await _userManager.UpdateAsync(data);

            return result.Succeeded;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}