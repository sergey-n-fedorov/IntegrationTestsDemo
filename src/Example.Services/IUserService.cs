using Example.Shared.Models;

namespace Example.Services;

public interface IUserService
{
    Task FetchAsync();
    Task<List<User>> GetAllAsync();
    Task DeleteAsync(int userId);
}