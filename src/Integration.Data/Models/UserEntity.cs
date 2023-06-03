namespace Integration.Data.Models;

public class UserEntity
{
    public long Id { get; set; }

    public string IntegrationId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;
}