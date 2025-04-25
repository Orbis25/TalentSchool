using FoundationKit.Repository.Services;
using TalentSchool.Application.EF.Repositories;
using TalentSchool.Domain.Models;
using TalentSchool.Infrastructure.Persistence;

namespace TalentSchool.Infrastructure.EF;

public class ModuleRepository : BaseRepository<ApplicationDbContext,Module>, IModuleRepository
{
    public ModuleRepository(ApplicationDbContext context) : base(context)
    {
    }
}