using System.Linq.Expressions;
using FoundationKit.Domain.Dtos.Paginations;
using Foundationkit.Extensions.Enumerations;
using Microsoft.EntityFrameworkCore;
using TalentSchool.Application.Core.Context;
using TalentSchool.Application.EF.Repositories;
using TalentSchool.Domain.Models;

namespace TalentSchool.Application.Services;

public interface IModuleService
{
    Task<Module?> Get(Expression<Func<Module, bool>> predicate, CancellationToken cancellationToken = default);
    Task<PaginationResult<Module>> GetAllAsync(Paginate paginate, CancellationToken cancellationToken = default);
    Task<bool> EditAsync(Module data, CancellationToken cancellationToken = default);

    Task<bool> SoftRemoveAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> CreateAsync(Module data, CancellationToken cancellationToken = default);
}

public class ModuleService : IModuleService
{
    private readonly IModuleRepository _moduleRepository;
    private readonly IContext _context;

    public ModuleService(IModuleRepository moduleRepository, IContext context)
    {
        _context = context;
        _moduleRepository = moduleRepository;
    }

    public Task<Module?> Get(Expression<Func<Module, bool>> predicate, CancellationToken cancellationToken = default) =>
        _moduleRepository.GetAll().Include(x => x.Contents).FirstOrDefaultAsync(predicate, cancellationToken);

    public async Task<PaginationResult<Module>> GetAllAsync(Paginate paginate,
        CancellationToken cancellationToken = default)
    {
        var items = _moduleRepository.GetAll()
            .Include(x => x.Contents)
            .Include(x => x.Progresses)
            .AsQueryable();

        if (!string.IsNullOrEmpty(paginate.Query))
        {
            items = items.Where(x => x.Title!.Contains(paginate.Query));
        }

        paginate.NoPaginate = true;
        var result = await items.PaginateAsync(paginate, cancellationToken);

        result.Query = paginate.Query;
        return result!;
    }

    public async Task<bool> EditAsync(Module input, CancellationToken cancellationToken = default)
    {
        try
        {
            var data = await _moduleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == input.Id, cancellationToken);

            if (data == null)
                return false;

            data.Title = input.Title;
            data.Category = input.Category;
            data.Contents = input.Contents;


            var result = await _moduleRepository.UpdateAsync(data, cancellationToken);

            return result != null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> SoftRemoveAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var data = await _moduleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (data == null)
                return false;

            data.IsDeleted = true;

            var result = await _moduleRepository.UpdateAsync(data, cancellationToken);

            return result != null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> CreateAsync(Module data, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _moduleRepository.CreateAsync(data, cancellationToken);

            if (result == null)
                return false;

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}