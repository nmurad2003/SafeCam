using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SafeCamApp.Models;
using SafeCamApp.ViewModels.AccountVMs;

namespace SafeCamApp.Controllers;

public class AccountController(RoleManager<IdentityRole> _roleManager, UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager) : Controller
{

    // RUN FIRST!!!
    #region initializing roles and admins
    //public async Task<IActionResult> CreateRoles()
    //{
    //    List<string> roleNames = ["admin", "member"];

    //    foreach (string roleName in  roleNames)
    //        await _roleManager.CreateAsync(new IdentityRole() { Name = roleName });        

    //    return Ok("Roles Created!");
    //}

    public async Task<IActionResult> CreateAdmins()
    {
        var admin = new AppUser()
        {
            // TODO: Add username
            FirstName = "admin",
            LastName = "admin",
            Email = "admin@code.edu.az",
            UserName="Admin"
        };

       var result= await _userManager.CreateAsync(admin, "admin1234");
        await _userManager.AddToRoleAsync(admin, "admin");

        return Ok(result);
    }
    #endregion

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Register(RegisterVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new AppUser()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        await _userManager.AddToRoleAsync(user, "user");

        return View();
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost, AutoValidateAntiforgeryToken]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: true);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Invalid email or password");
            return View(model);
        }

        return RedirectToAction("Home", "Index");
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Home", "Index");
    }
}
