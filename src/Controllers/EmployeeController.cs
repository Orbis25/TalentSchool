using FoundationKit.Core.Controllers;
using FoundationKit.Domain.Dtos.Paginations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TalentSchool.Application.Dtos.Users;
using TalentSchool.Application.Services.UsersService;
using TalentSchool.Domain.Models;

namespace TalentSchool.Controllers;

[Route("[controller]")]
[Authorize(Roles = "Administrador")]
public class EmployeeController : Controller
{
    private readonly IUserService _userService;

    public EmployeeController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index([FromQuery] Paginate paginate, CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetAllAsync(paginate, cancellationToken);

        return View(result);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUser user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        var result = await _userService.RegisterUserAsync(user);

        if (result.Success)
        {
            TempData["Success"] = "User created successfully.";
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, result.Message!);
        return View(user);
    }

    [HttpGet("Edit/{id}")]
    public async Task<IActionResult> Edit([FromRoute] string id)
    {
        var result = await _userService.Get(x => x.Id == id);

        if (result == null)
        {
            return NotFound();
        }

        return View(result);
    }

    [HttpPost("Edit/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit([FromRoute] string id, User user,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        user.Id = id;
        var updateResult = await _userService.EditAsync(user, cancellationToken);

        if (updateResult)
        {
            TempData["Success"] = "User updated successfully.";
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to update employee.");
        return View(user);
    }

    [HttpGet("Delete/{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var result = await _userService.Get(x => x.Id == id);

        if (result == null)
        {
            return NotFound();
        }

        return View(result);
    }

    [HttpPost("Delete/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete([FromRoute] string id, CancellationToken cancellationToken = default)
    {
        var user = await _userService.Get(x => x.Id == id);

        if (user == null)
        {
            return NotFound();
        }

        var updateResult = await _userService.SoftRemoveAsync(id, cancellationToken);

        if (updateResult)
        {
            TempData["Success"] = "User deleted successfully.";
            return RedirectToAction("Index");
        }

        ModelState.AddModelError(string.Empty, "Failed to delete employee.");
        return View(user);
    }
}