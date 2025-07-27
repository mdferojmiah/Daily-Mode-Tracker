using System.Diagnostics;
using System.Globalization;
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
    private readonly IQuoteService _quoteService;
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService  _emailService;
    public HomeController(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager, IEmailService emailService, IQuoteService quoteService)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _quoteService = quoteService;
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        
        var recentMoodEntries = await _context.MoodEntries
            .OrderByDescending(m => m.Created)
            .Take(5) 
            .ToListAsync();
        ViewBag.RecentMoodEntries = recentMoodEntries;
        ViewBag.LayoutMoodHistory = recentMoodEntries;
        ViewBag.Date = DateTime.Now;
        try
        {
            ViewBag.randomQuote = await _quoteService.GetRandomQuote();
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
                                          $"{user.FullName} has recorded a mood of '{model.Tag}' " +
                                          $"with the description: '{model.Description}'.\n\n" +
                                          "Please reach out to them if you are able. " +
                                          "This is an automated message from Mood Tracker App.\n\n" +
                                          "Well wishes,\n" +
                                          "Daily Mood Tracker Team <3\n";
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
        var user = await _userManager.GetUserAsync(User);
        List<MoodEntry> allMoodEntries = new List<MoodEntry>();
        if (user != null)
        {
            allMoodEntries = await _context.MoodEntries
                .OrderByDescending(m => m.Created)
                .ToListAsync();
                
        }
        var cultureInfo = CultureInfo.CurrentCulture;
        var calendar = cultureInfo.Calendar;
        var currentWeek = calendar.GetWeekOfYear(DateTime.Now, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek);
        
        var entriesThisWeek = allMoodEntries.Count(e => calendar
            .GetWeekOfYear(e.Created, cultureInfo.DateTimeFormat.CalendarWeekRule, cultureInfo.DateTimeFormat.FirstDayOfWeek) == currentWeek && e.Created.Year == DateTime.Now.Year);
        MoodTagOptions dominantMood = MoodTagOptions.Happy;
        if (allMoodEntries.Any())
        {
            dominantMood = allMoodEntries
                .GroupBy(e => e.Tag)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefault(); 
        }
        int totalDaysInTrackingPeriod = 7;
        var startDate = DateTime.Today.AddDays(-totalDaysInTrackingPeriod + 1);
        var consistencyIn7Days = allMoodEntries
            .Where(e => e.Created.Date >= startDate && e.Created.Date <= DateTime.Today)
            .Select(e => e.Created.Date)
            .Distinct()
            .Count();
        
        var viewModel = new MoodTrackerViewModel
        {
            AllMoodEntries = allMoodEntries,
            EntriesThisWeek = entriesThisWeek,
            DominantMood = dominantMood,
            ConsistencyIn7Days = consistencyIn7Days,
        };
        
        var recentMoodEntries = allMoodEntries.Take(5);
        ViewBag.RecentMoodEntries = recentMoodEntries;
        ViewBag.LayoutMoodHistory = recentMoodEntries;
        return View(viewModel);
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