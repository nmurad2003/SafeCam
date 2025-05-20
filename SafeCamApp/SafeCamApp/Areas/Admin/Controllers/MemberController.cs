using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SafeCamApp.Contexts;
using SafeCamApp.Models;
using SafeCamApp.ViewModels.DesignationVMs;
using SafeCamApp.ViewModels.MemberVMs;

namespace SafeCamApp.Areas.Admin.Controllers;

[Area("Admin")]
public class MemberController(SafeCamDbContext _context, IWebHostEnvironment _env) : Controller
{
    public async Task<IActionResult> Index()
    {
        List<MemberGetVM> vms = await _context.Members.Select(m => new MemberGetVM()
        {
            Id = m.Id,
            FirstName = m.FirstName,
            LastName = m.LastName,
            ImagePath = m.ImagePath,
            Designation = m.Designation,
        }).ToListAsync();

        return View(vms);
    }

    public async Task<IActionResult> Create()
    {
        await FillDesignationsToViewBagAsync();
        return View();
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Create(MemberCreateVM model)
    {
        if (!ModelState.IsValid)
        {
            await FillDesignationsToViewBagAsync();
            return View(model);
        }

        string? imagePath = null;
        if (model.Image != null)
        {
            if (model.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Image size cannot exceed 2 MBs!");
                await FillDesignationsToViewBagAsync();
                return View(model);
            }

            if (!model.Image.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("Image", "Only image files are accepted!");
                await FillDesignationsToViewBagAsync();
                return View(model);
            }

            imagePath = await CopyToNewImagePathAsync(model.Image);
        }

        var entity = new Member()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            DesignationId = model.DesignationId,
            ImagePath = imagePath,
        };

        await _context.Members.AddAsync(entity);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        await FillDesignationsToViewBagAsync();

        Member? entity = await _context.Members.FindAsync(id);
        if (entity == null)
            return NotFound();

        var model = new MemberUpdateVM()
        {
            Id = entity!.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            ImagePath = entity.ImagePath,
            DesignationId = entity.DesignationId,
        };

        return View(model);
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Update(MemberUpdateVM model)
    {
        if (!ModelState.IsValid)
        {
            await FillDesignationsToViewBagAsync();
            return View(model);
        }

        Member? entity = await _context.Members.FindAsync(model.Id);
        if (entity == null)
            return NotFound();

        string? imagePath = entity.ImagePath;
        if (model.Image != null)
        {
            if (model.Image.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Image size cannot exceed 2 MBs!");
                await FillDesignationsToViewBagAsync();
                return View(model);
            }

            if (!model.Image.ContentType.StartsWith("image"))
            {
                ModelState.AddModelError("Image", "Only image files are accepted!");
                await FillDesignationsToViewBagAsync();
                return View(model);
            }

            if (entity.ImagePath != null)
                await CopyToExistingImagePathAsync(model.Image, entity.ImagePath);
            else
                imagePath = await CopyToNewImagePathAsync(model.Image);
        }

        entity.FirstName = model.FirstName;
        entity.LastName = model.LastName;
        entity.DesignationId = model.DesignationId;
        entity.ImagePath = imagePath;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        Member? entity = await _context.Members.FindAsync(id);
        if (entity == null)
            return NotFound();

        if (entity.ImagePath != null)
        {
            string fullPath = _env.WebRootPath + entity.ImagePath;
            System.IO.File.Delete(fullPath);
        }

        _context.Members.Remove(entity);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));

    }

    #region Utility Methods
    public async Task FillDesignationsToViewBagAsync()
    {
        ViewBag.Designations = await _context.Designations.Select(c => new DesignationGetVM()
        {
            Id = c.Id,
            Name = c.Name
        }).ToListAsync();
    }

    public async Task<string> CopyToNewImagePathAsync(IFormFile image)
    {
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
        string fullPath = Path.Combine(_env.WebRootPath, "uploads", fileName);

        using var fs = new FileStream(fullPath, FileMode.Create);
        await image.CopyToAsync(fs);

        return "/uploads/" + fileName;
    }

    public async Task CopyToExistingImagePathAsync(IFormFile image, string imagePath)
    {
        string fullPath = _env.WebRootPath + imagePath;

        using var fs = new FileStream(fullPath, FileMode.Create);
        await image.CopyToAsync(fs);
    }

    #endregion
}
