using Refit;

namespace Example.Services.External;

public interface IExternalServiceClient
{
    [Get("/api/v1/User")]
    public Task<List<ExternalUser>> GetAllUsersAsync();
    
    [Post("/api/v1/User")]
    public Task UpdateUser(ExternalUser user);
}