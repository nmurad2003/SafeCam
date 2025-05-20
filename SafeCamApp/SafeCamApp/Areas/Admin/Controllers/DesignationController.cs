using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCamApp.Contexts;
using SafeCamApp.Models;
using SafeCamApp.ViewModels.DesignationVMs;

namespace SafeCamApp.Areas.Admin.Controllers;

[Area("Admin")]
public class DesignationController(SafeCamDbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<DesignationGetVM> vms = await _context.Designations.Select(c => new DesignationGetVM()
        {
            Id = c.Id,
            Name = c.Name,
        }).ToListAsync();

        return View(vms);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(DesignationCreateVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var entity = new Designation() { Name =  model.Name };
        await _context.Designations.AddAsync(entity);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        Designation? entity = await _context.Designations.FindAsync(id);
        
        if (entity == null)
            return NotFound();

        var model = new DesignationUpdateVM()
        {
            Id = entity.Id,
            Name = entity.Name,
        };

        return View(model);
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Update(DesignationUpdateVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        Designation? entity = await _context.Designations.FindAsync(model.Id);
        if (entity == null)
            return NotFound();

        entity.Name = model.Name;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Designation? entity = await _context.Designations.FindAsync(id);
        if (entity == null)
            return NotFound();

        _context.Designations.Remove(entity);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
