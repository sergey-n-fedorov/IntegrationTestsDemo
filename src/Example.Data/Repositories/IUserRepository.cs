using Example.Data.Entities;

namespace Example.Data.Repositories;

public interface IUserRepository
{
    Task<List<UserEntity>> GetAllUsersAsync();
    Task<UserEntity?> Find(int userId);
    Task AddRangeAsync(IEnumerable<UserEntity> entities);
    Task DeleteUserAsync(UserEntity userEntity);
}