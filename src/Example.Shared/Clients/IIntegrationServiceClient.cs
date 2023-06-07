using Example.Shared.Models;
using Refit;

namespace Example.Shared.Clients;

public interface IIntegrationServiceClient
{
    [Get("/api/v1/User")]
    Task<List<User>> GetUsersAsync();
    
    [Post("/api/v1/User/fetch")]
    Task FetchUsersAsync();
    
    [Post("/api/v1/User")]
    Task UpdateUserAsync(User user);
}