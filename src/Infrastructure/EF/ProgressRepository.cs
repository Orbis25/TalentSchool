using FoundationKit.Repository.Services;
using TalentSchool.Application.EF.Repositories;
using TalentSchool.Domain.Models;
using TalentSchool.Infrastructure.Persistence;

namespace TalentSchool.Infrastructure.EF;

public class ProgressRepository : BaseRepository<ApplicationDbContext,Progress>, IProgressRepository
{
    public ProgressRepository(ApplicationDbContext context) : base(context)
    {
    }
}