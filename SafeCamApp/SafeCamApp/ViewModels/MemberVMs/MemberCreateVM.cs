namespace SafeCamApp.ViewModels.MemberVMs;

public class MemberCreateVM
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IFormFile? Image { get; set; }
    public int DesignationId { get; set; }
}
