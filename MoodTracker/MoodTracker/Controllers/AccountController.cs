using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoodTracker.Models.Entities;
using MoodTracker.Models.ViewModels;

namespace MoodTracker.Controllers;

public class AccountController: Controller
{
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;
    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    //GET: account/signin
    [HttpGet]
    public IActionResult SignIn()
    {
        return View();
    }
    
    //POST: account/signin
    [HttpPost]
    public async Task<IActionResult> SignIn(SignInViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email!, model.Password!, true, false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }
        return View(model);
    }
    
    //GET: account/signup
    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }
    
    //POST: account/signup
    [HttpPost]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                TrustedPersonsEmail = model.TrustedPersonsEmail,
                TrustedPersonsNumber = model.TrustedPersonsNumber,
                UserName = model.Email
            };
            var result = await _userManager.CreateAsync(user, model.Password!);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, true);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("SignIn", "Account");
    }
}