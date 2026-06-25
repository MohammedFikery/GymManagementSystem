using GymManagementSystem.BLL.ViewModels.Login;
using GymManagementSystem.Controllers;
using GymManagementSystem.DAL.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementSystem.PL.Controllers;

public class AccountController:Controller
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AccountController> _logger;

    public AccountController(SignInManager<AppUser> signInManager,UserManager<AppUser> userManager,ILogger<AccountController> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    #region Login
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (_signInManager.IsSignedIn(User))return RedirectToAction("Index", "Home");
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken] 
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (!ModelState.IsValid)return View(model);
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            ModelState.AddModelError("InvalidLogin", "Invalid login attempt.");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password,isPersistent: model.RememberMe,lockoutOnFailure: true);       

        if (result.Succeeded)  
        {
            _logger.LogInformation("User {Email} logged in.", model.Email);
            return RedirectToAction(nameof(HomeController.Index),"Home");
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning("User {Email} account locked out.", model.Email);
            ModelState.AddModelError("InvalidLogin", "Account locked. Try again later.");
            return View(model);
        }

        if (result.IsNotAllowed)
        {
            ModelState.AddModelError("InvalidLogin", "Login not allowed. Confirm your email first.");
            return View(model);
        }

        ModelState.AddModelError("InvalidLogin", "Invalid login attempt.");
        return View(model);
    }

    #endregion

    #region Logout

    [HttpPost]
    [Authorize] 
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        return RedirectToAction(nameof(Login));
    }

    #endregion

    #region AccessDenied

    [HttpGet]
    public IActionResult AccessDenied(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    #endregion

}
