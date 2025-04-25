using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TalentSchool.Domain.Models;

namespace TalentSchool.Controllers;

[AllowAnonymous]
[Route("Account")]
public class AccountController : Controller
{
    private readonly SignInManager<User> _signInManager;

    public AccountController(SignInManager<User> signInManager)
    {
        _signInManager = signInManager;
    }


    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index","Home");
    }
}