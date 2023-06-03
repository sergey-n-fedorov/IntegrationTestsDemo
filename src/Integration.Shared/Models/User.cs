namespace Integration.Shared.Models;

public class User
{
    public long Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;
}