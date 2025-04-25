using FoundationKit.Domain.Dtos.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TalentSchool.Application.Core.Context;
using TalentSchool.Application.Dtos;
using TalentSchool.Application.Services;
using TalentSchool.Application.Services.UsersService;
using TalentSchool.Domain.Enums;
using TalentSchool.ViewModels;

namespace TalentSchool.Controllers;

[Authorize]
[Route("[controller]")]
public class ProgressController : Controller
{
    private readonly IProgressService _service;
    private readonly IUserService _userService;
    private readonly IModuleService _moduleService;


    public ProgressController(IProgressService service,
        IUserService userService,
        IModuleService moduleService)
    {
        _service = service;
        _userService = userService;
        _moduleService = moduleService;
    }

    [HttpGet("Assign")]
    public async Task<IActionResult> Assign(Paginate paginate)
    {
        paginate.NoPaginate = true;

        await LoadAssignItemsAsync(paginate).ConfigureAwait(false);

        return View(new AssignProgressViewModel());
    }

    [HttpPost("Assign")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Assign(AssignProgressViewModel model)
    {
        if (!ModelState.IsValid)
        {
            await LoadAssignItemsAsync().ConfigureAwait(false);
            return View(model);
        }

        var exist = await _service.ExistAsync(x => x.ModuleId == model.ModuleId && x.UserId == model.UserId);

        if (exist)
        {
            ModelState.AddModelError(string.Empty, "Ya fue asignado el modulo a este usuario.");
            await LoadAssignItemsAsync().ConfigureAwait(false);
            return View(model);
        }

        var result = await _service.CreateAsync(new()
        {
            UserId = model.UserId,
            ModuleId = model.ModuleId,
            Status = model.Status
        }).ConfigureAwait(false);

        if (result)
        {
            TempData["Success"] = "Modulo asignado correctamente.";
            return RedirectToAction("Index", "Module");
        }

        ModelState.AddModelError(string.Empty, "Error al asignar el modulo.");
        return View(model);
    }

    [HttpGet("Take/{id:guid}", Name = "Take")]
    public async Task<IActionResult> TakeModule(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _service.TakeModuleAsync(id, cancellationToken);
        if (result == null)
        {
            return NotFound();
        }

        return View(result);
    }

    [HttpPost("Navigate")]
    public async Task<IActionResult> Navigate(NavigateInput input,
        CancellationToken cancellationToken = default)
    {
        var result =
            await _service.NextContentAsync(input.ModuleId, input.CurrentIndex, input.Direction ?? "",
                cancellationToken);

        if (!result)
        {
            return BadRequest("Error al navegar por el contenido.");
        }

        return RedirectToRoute("Take", new { id = input.ModuleId });
    }

    private async Task LoadAssignItemsAsync(Paginate? paginate = default)
    {
        paginate ??= new()
        {
            NoPaginate = true,
            Page = 1,
        };

        var users = await _userService.GetAllAsync(paginate);

        var usersList = users.Results
            .Where(x => x.Type == Domain.Enums.UserType.Employee)
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FullName
            }).ToList();

        var modules = await _moduleService.GetAllAsync(paginate);
        var modulesList = modules.Results
            .Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.Title
            }).ToList();

        ViewBag.Users = usersList;
        ViewBag.Modules = modulesList;
    }
}