using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCamApp.Contexts;
using SafeCamApp.Models;
using SafeCamApp.ViewModels.MemberVMs;
using System.Diagnostics;

namespace SafeCamApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly SafeCamDbContext _context;

    public HomeController(ILogger<HomeController> logger, SafeCamDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        List<MemberGetVM> vms = await _context.Members.Select(m => new MemberGetVM()
        {
            Id = m.Id,
            FirstName = m.FirstName,
            LastName = m.LastName,
            Designation = m.Designation,
            ImagePath = m.ImagePath,
        }).ToListAsync();

        return View(vms);
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
