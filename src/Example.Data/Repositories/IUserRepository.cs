using Example.Data.Entities;

namespace Example.Data.Repositories;

public interface IUserRepository
{
    Task AddUser(UserEntity userEntity);
    Task<List<UserEntity>> GetAllUsersAsync();
    Task<UserEntity?> Find(int userId);
    Task UpdateUserAsync(UserEntity userEntity);
    Task AddRangeAsync(IEnumerable<UserEntity> entities);
    Task DeleteUserAsync(UserEntity userEntity);
}