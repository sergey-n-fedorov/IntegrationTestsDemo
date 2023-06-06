using Integration.Shared.Models;

namespace Integration.Services;

public interface IUserService
{
    Task FetchAsync();
    Task<List<User>> GetAllAsync();
    Task UpdateUserAsync(User user);
    Task DeleteAsync(int userId);
}