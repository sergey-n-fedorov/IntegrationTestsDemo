using Refit;

namespace Example.Services.External;

public interface IExternalServiceClient
{
    [Get("/api/v1/User")]
    public Task<List<ExternalUser>> GetAllUsersAsync();
    
    [Delete("/api/v1/User")]
    public Task DeleteUser(string userId);
}