using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoodTracker.Models;
using MoodTracker.Models.ViewModels;

namespace MoodTracker.Controllers;

public class HomeController : Controller
{
    
    public HomeController()
    {
    }

    [Authorize]
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    [Authorize]
    public IActionResult MoodTracker()
    {
        return View();
    }

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