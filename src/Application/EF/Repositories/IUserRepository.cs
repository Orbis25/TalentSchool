using FoundationKit.Repository.Interfaces;
using TalentSchool.Domain.Models;

namespace TalentSchool.Application.EF.Repositories;

public interface IUserRepository
{
    IQueryable<User> Get();
}