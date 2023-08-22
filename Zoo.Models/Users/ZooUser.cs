namespace Zoo.Models.Users;

public class ZooUser : IdentityUser
{

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string FullName => FirstName + LastName;
}
