using SafeCamApp.Models;

namespace SafeCamApp.ViewModels.MemberVMs;

public class MemberGetVM
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ImagePath { get; set; }
    public Designation Designation { get; set; }
}
