namespace Integration.Services.External;

public interface IExternalServiceClient
{
    public Task<List<ExternalUser>> GetAllUsersAsync();
    
    public Task UpdateUser(ExternalUser user);
}