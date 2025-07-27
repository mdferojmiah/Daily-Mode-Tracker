using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodTracker.Data;
using MoodTracker.Models;
using MoodTracker.Models.Entities;
using MoodTracker.Models.Enums;
using MoodTracker.Models.ViewModels;
using MoodTracker.ServiceContracts;

namespace MoodTracker.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService  _emailService;
    public HomeController(IHttpClientFactory clientFactory, AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService)
    {
        _clientFactory = clientFactory;
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        ViewBag.Date = DateTime.Now;
        Quote? randomQuote = null;
        try
        {
            HttpClient client = _clientFactory.CreateClient("ZenQuoteAPI");
            HttpResponseMessage response = await client.GetAsync("/api/random");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            List<Quote>? quotes = JsonSerializer.Deserialize<List<Quote>>(responseBody);
            if (quotes != null)
            {
                randomQuote = quotes[0];
            }
        }
        catch (HttpRequestException ex)
        {
            TempData["ErrorMessage"] = "Could not load quote. Please try again later.";
        }
        catch (JsonException ex)
        {
            TempData["ErrorMessage"] = "Error processing quote data.";
        }
        catch (Exception ex)
        {
            TempData["ErrorMessage"] = "An unexpected error occurred.";
        }
        var recentMoodEntries = await _context.MoodEntries
            .OrderByDescending(m => m.Created)
            .Take(5) 
            .ToListAsync();
        ViewBag.RecentMoodEntries = recentMoodEntries;
        ViewBag.LayoutMoodHistory = recentMoodEntries;
        ViewBag.randomQuote = randomQuote;
        return View();
    }
    
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Index(MoodEntryViewModel model)
    {
        if (ModelState.IsValid)
        {
            var moodEntry = new MoodEntry
            {  
                Id = Guid.NewGuid(),
                Tag = model.Tag,
                Description = model.Description,
                Created = DateTime.Now,
                
            };
            await _context.AddAsync(moodEntry);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                TempData["Success"] = "Mood entry has been added successfully";
            }
            //sending emails if mood is bad
            User? user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                if (model.Tag == MoodTagOptions.Depressed || model.Tag == MoodTagOptions.Frustrated)
                {
                    string subject = $"Daily Mood Tracker Alert: {user.UserName} is feeling {model.Tag}";
                    string body = $"Hello {user.TrustedPersonsName ?? "Trusted Person"},\n\n" +
                                          $"{user.UserName} has recorded a mood of '{model.Tag}' " +
                                          $"with the description: '{model.Description}'.\n\n" +
                                          "Please reach out to them if you are able. This is an automated message from Mood Tracker App";
                    if (!string.IsNullOrEmpty(user.TrustedPersonsEmail))
                    {
                        await _emailService.SendEmail(user.TrustedPersonsEmail, subject, body);
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
        ModelState.AddModelError("", "Data can not be Added");
        var recentMoodEntries = await _context.MoodEntries
            .OrderByDescending(m => m.Created)
            .Take(5) 
            .ToListAsync();
        ViewBag.RecentMoodEntries = recentMoodEntries;
        ViewBag.LayoutMoodHistory = recentMoodEntries;
        return View(model);
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> MoodTracker()
    {
        var recentMoodEntries = await _context.MoodEntries
            .OrderByDescending(m => m.Created)
            .Take(5) 
            .ToListAsync();
        ViewBag.RecentMoodEntries = recentMoodEntries;
        ViewBag.LayoutMoodHistory = recentMoodEntries;
        return View();
    }
    
    [HttpGet]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}