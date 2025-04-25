using TalentSchool.Application.EF.Repositories;
using TalentSchool.Domain.Models;
using TalentSchool.Infrastructure.Persistence;

namespace TalentSchool.Infrastructure.EF;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<User> Get() => _dbContext.Users.AsQueryable();
}