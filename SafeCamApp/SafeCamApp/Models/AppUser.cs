using Microsoft.AspNetCore.Identity;

namespace SafeCamApp.Models;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
