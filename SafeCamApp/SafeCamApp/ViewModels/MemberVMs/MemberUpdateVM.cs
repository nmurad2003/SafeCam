namespace SafeCamApp.ViewModels.MemberVMs;

public class MemberUpdateVM
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ImagePath { get; set; }
    public IFormFile? Image { get; set; }
    public int DesignationId { get; set; }
}
