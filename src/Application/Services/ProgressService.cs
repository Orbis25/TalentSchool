using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TalentSchool.Application.Core.Context;
using TalentSchool.Application.EF.Repositories;
using TalentSchool.Domain.Enums;
using TalentSchool.Domain.Models;
using TalentSchool.ViewModels;

namespace TalentSchool.Application.Services;

public interface IProgressService
{
    Task<bool> CreateAsync(Progress data, CancellationToken cancellationToken = default);
    Task<bool> ExistAsync(Expression<Func<Progress, bool>> expression, CancellationToken cancellationToken = default);

    Task<Progress?> GetAsync(Expression<Func<Progress, bool>> expression,
        CancellationToken cancellationToken = default);

    Task<TakeModuleViewModel?> TakeModuleAsync(Guid moduleId,
        CancellationToken cancellationToken = default);

    Task<bool> NextContentAsync(Guid moduleId, int currentIndex, string direction,
        CancellationToken cancellationToken = default);
}

public class ProgressService : IProgressService
{
    private readonly IProgressRepository _repository;
    private readonly IModuleRepository _moduleRepository;
    private readonly IContext _context;

    public ProgressService(IProgressRepository repository, IContext context, IModuleRepository moduleRepository)
    {
        _moduleRepository = moduleRepository;

        _repository = repository;
        _context = context;
    }

    public async Task<bool> CreateAsync(Progress data, CancellationToken cancellationToken = default)
    {
        try
        {
            await _repository.CreateAsync(data, cancellationToken);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<bool> ExistAsync(Expression<Func<Progress, bool>> expression,
        CancellationToken cancellationToken = default)
        => await _repository.ExistAsync(expression, cancellationToken);

    public async Task<Progress?> GetAsync(Expression<Func<Progress, bool>> expression,
        CancellationToken cancellationToken = default)
        => await _repository.GetAll().FirstOrDefaultAsync(expression, cancellationToken);

    public async Task<TakeModuleViewModel?> TakeModuleAsync(Guid moduleId,
        CancellationToken cancellationToken = default)
    {
        var progress = await _repository.GetOneAsync(p =>
            p.UserId == _context.UserId && p.ModuleId == moduleId, cancellationToken);

        if (progress == null)
        {
            return null;
        }

        var module = await _moduleRepository.GetAll().Include(x => x.Contents)
            .FirstOrDefaultAsync(m => m.Id == moduleId, cancellationToken);

        if (module?.Contents == null || module.Contents.Count == 0)
        {
            return null;
        }

        // Si el progreso está en estado "No iniciado", actualizarlo a "En progreso"
        if (progress.Status == ProgressStatus.New)
        {
            progress.Status = ProgressStatus.InProgress;
            progress.CurrentContentIndex = 1;
        }

        // Obtener el contenido actual según el índice
        var currentIndex = progress.CurrentContentIndex ?? 1;
        var currentContent = module.Contents
            .OrderBy(c => c.Index)
            .FirstOrDefault(c => c.Index == currentIndex);

        if (currentContent == null)
        {
            currentContent = module.Contents.OrderBy(c => c.Index).First();
            progress.CurrentContentIndex = currentContent.Index;
        }

        var viewModel = new TakeModuleViewModel
        {
            UserId = _context.UserId,
            ModuleId = moduleId,
            ModuleTitle = module.Title,
            CurrentContentIndex = currentIndex,
            CurrentContent = currentContent,
            TotalContents = module.Contents.Count,
            Status = progress.Status,
            CompletedAt = progress.CompletedAt,
            HasPrevious = module.Contents.Any(c => c.Index < currentIndex),
            HasNext = module.Contents.Any(c => c.Index > currentIndex),
            IsLastContent = currentIndex == module.Contents.Max(c => c.Index)
        };
        return viewModel;
    }

    public async Task<bool> NextContentAsync(Guid moduleId, int currentIndex, string direction,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var progress = await _repository.GetAll().FirstOrDefaultAsync(p =>
                p.UserId == _context.UserId && p.ModuleId == moduleId, cancellationToken);

            if (progress == null)
                return false;

            var module = await _moduleRepository.GetAll().Include(x => x.Contents)
                .FirstOrDefaultAsync(m => m.Id == moduleId, cancellationToken);

            if (module?.Contents == null || module.Contents.Count == 0)
            {
                return false;
            }

            var orderedContents = module.Contents.OrderBy(c => c.Index).ToList();
            progress.Status = ProgressStatus.InProgress;

            switch (direction)
            {
                case "next":
                {
                    var nextContent = orderedContents.FirstOrDefault(c => c.Index > currentIndex);
                    if (nextContent != null)
                    {
                        progress.CurrentContentIndex = nextContent.Index;
                    }

                    break;
                }
                case "prev":
                {
                    var prevContent = orderedContents
                        .OrderByDescending(c => c.Index)
                        .FirstOrDefault(c => c.Index < currentIndex);

                    if (prevContent != null)
                    {
                        progress.CurrentContentIndex = prevContent.Index;
                    }

                    break;
                }
                case "complete":
                    progress.Status = ProgressStatus.Completed;
                    progress.CompletedAt = DateTime.Now;
                    break;
            }

            return await _repository.UpdateAsync(progress, cancellationToken) != null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}