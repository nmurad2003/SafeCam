namespace SafeCamApp.Models;

public class Designation : BaseEntity
{
    public string Name { get; set; }
    public IEnumerable<Member> Members { get; set; }
}
