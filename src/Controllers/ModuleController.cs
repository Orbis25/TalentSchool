using FoundationKit.Domain.Dtos.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSchool.Application.Services;
using TalentSchool.Domain.Models;

namespace TalentSchool.Controllers;

[Authorize]
[Route("[controller]")]
public class ModuleController : Controller
{
    private readonly IModuleService _service;

    public ModuleController(IModuleService moduleService)
    {
        _service = moduleService;
    }


    [HttpGet("index")]
    public async Task<IActionResult> Index(Paginate paginate, CancellationToken cancellationToken = default)
    {
        return View(await _service.GetAllAsync(paginate, cancellationToken));
    }


    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Module data, CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View(data);
        }

        var result = await _service.CreateAsync(data, cancellationToken);

        if (result)
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to create module.");
        return View(data);
    }

    [HttpGet("Edit/{id:guid}")]
    public async Task<IActionResult> Edit([FromRoute] Guid id)
    {
        var result = await _service.Get(x => x.Id == id);

        if (result == null)
        {
            return NotFound();
        }

        return View(result);
    }

    [HttpPost("Edit/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] Guid id, Module data,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View(data);
        }

        data.Id = id;
        var updateResult = await _service.EditAsync(data, cancellationToken);

        if (updateResult)
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to update employee.");
        return View(data);
    }

    [HttpGet("Delete/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await _service.Get(x => x.Id == id);

        if (result == null)
        {
            return NotFound();
        }

        return View(result);
    }

    [HttpPost("Delete/{id:guid}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _service.Get(x => x.Id == id, cancellationToken);

        if (user == null)
        {
            return NotFound();
        }

        var updateResult = await _service.SoftRemoveAsync(id, cancellationToken);

        if (updateResult)
        {
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to delete employee.");
        return View(user);
    }
}