namespace Integration.Data.Entities;

public class UserEntity
{
    public int Id { get; set; }
    public string ExternalId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Address { get; set; } = null!;
}