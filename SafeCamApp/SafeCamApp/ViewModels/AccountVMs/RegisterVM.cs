using System.ComponentModel.DataAnnotations;

namespace SafeCamApp.ViewModels.AccountVMs;

public class RegisterVM
{
    [Required(ErrorMessage = "Firs tName is required!")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last Name is required!")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required!")]
    [EmailAddress(ErrorMessage = "Invalid email address!")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required!")]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Confirm Password is required!")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}
