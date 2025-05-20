namespace SafeCamApp.Models;

public class Member : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? ImagePath { get; set; }
    public int DesignationId { get; set; }
    public Designation Designation { get; set; }
}
