using Integration.Data.Entities;

namespace Integration.Data.Repositories;

public interface IUserRepository
{
    Task AddUser(UserEntity userEntity);
    Task<List<UserEntity>> GetAllUsersAsync();
    Task<UserEntity?> Find(int userId);
    Task UpdateUserAsync(UserEntity userEntity);
    Task AddRangeAsync(IEnumerable<UserEntity> entities);
}