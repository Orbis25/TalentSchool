using System.Linq.Expressions;
using FoundationKit.Domain.Dtos.Paginations;
using TalentSchool.Application.Dtos.Core;
using TalentSchool.Application.Dtos.Users;
using TalentSchool.Domain.Models;
using TalentSchool.Infrastructure.Seeds;

namespace TalentSchool.Application.Services.UsersService;

public interface IUserService : ISeed
{
    Task<BaseResponse<bool>> RegisterUserAsync(CreateUser user, bool system = false);
    Task<BaseResponse<bool>> SignInUserAsync(string email, string password);
    Task<User?> Get(Expression<Func<User, bool>> predicate);
    Task<PaginationResult<User>> GetAllAsync(Paginate paginate, CancellationToken cancellationToken = default);
    Task<bool> EditAsync(User user, CancellationToken cancellationToken = default);

    Task<bool> SoftRemoveAsync(string id, CancellationToken cancellationToken = default);
}