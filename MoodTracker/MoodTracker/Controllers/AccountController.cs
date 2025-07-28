using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodTracker.Data;
using MoodTracker.Models.Entities;
using MoodTracker.Models.Enums;
using MoodTracker.Models.ViewModels;

namespace MoodTracker.Controllers;

public class AccountController : Controller
{
    private UserManager<User> _userManager;
    private SignInManager<User> _signInManager;
    private AppDbContext  _context;

    public AccountController(SignInManager<User> signInManager, UserManager<User> userManager, AppDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _context = context;
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
                TrustedPersonsName = model.TrustedPersonsName,
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

    //GET: account/profile
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        User? user = await _userManager.GetUserAsync(User);
        ProfileViewModel model = new ProfileViewModel();
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Session Expired! Login again!");
        }else
        {
            model.UserName = user.UserName;
            model.FullName = user.FullName;
            model.Email = user.Email;
            model.TrustedPersonsName = user.TrustedPersonsName;
            model.TrustedPersonsEmail = user.TrustedPersonsEmail;
            model.TrustedPersonsNumber = user.TrustedPersonsNumber;
            model.Gender = user.Gender;
            model.Birthday = user.Birthday;
        }
        var recentMoodEntries = await _context.MoodEntries
            .Where(u => u.UserId == user!.Id)
            .OrderByDescending(m => m.Created)
            .Take(5)
            .ToListAsync();
        ViewBag.RecentMoodEntries = recentMoodEntries;
        ViewBag.LayoutMoodHistory = recentMoodEntries;
        return View(model);
    }
    
    //GET: account/changeInformation
    [HttpGet]
    public async Task<IActionResult> ChangeInformation()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        var model = new ChangeInformationViewModel
        {
            FullName = user.FullName, 
            TrustedPersonsName = user.TrustedPersonsName,
            TrustedPersonsEmail = user.TrustedPersonsEmail,
            TrustedPersonsNumber = user.TrustedPersonsNumber,
            Gender = user.Gender, 
            Birthday = user.Birthday
        };
        var recentMoodEntries = await _context.MoodEntries
            .OrderByDescending(m => m.Created)
            .Take(5)
            .ToListAsync();
        ViewBag.RecentMoodEntries = recentMoodEntries;
        ViewBag.LayoutMoodHistory = recentMoodEntries;
        return View(model);
    }

    //Post: account/changeInformation
    [HttpPost]
    public async Task<IActionResult> ChangeInformation(ChangeInformationViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var user = await _userManager.GetUserAsync(User);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "User not found.");
            return View(model);
        }
        user.TrustedPersonsName = model.TrustedPersonsName;
        user.TrustedPersonsEmail = model.TrustedPersonsEmail;
        user.TrustedPersonsNumber = model.TrustedPersonsNumber;
        user.Gender = model.Gender;
        user.Birthday = model.Birthday;
        
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        
        if (!string.IsNullOrEmpty(model.NewPassword)) // Check if a new password was actually entered
        {
            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (removeResult.Succeeded)
            {
                var addResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (!addResult.Succeeded)
                {
                    foreach (var error in addResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
            }
            else
            {
                foreach (var error in removeResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }
            await _signInManager.SignOutAsync();
            await _signInManager.SignInAsync(user, isPersistent: true);
        }
        TempData["SuccessMessage"] = "Profile information updated successfully!";
        var recentMoodEntries = await _context.MoodEntries
            .OrderByDescending(m => m.Created)
            .Take(5)
            .ToListAsync();
        ViewBag.RecentMoodEntries = recentMoodEntries;
        ViewBag.LayoutMoodHistory = recentMoodEntries;
        return RedirectToAction("Profile", "Account");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("SignIn", "Account");
    }
}