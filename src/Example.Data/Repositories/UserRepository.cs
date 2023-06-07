using Example.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Example.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IntegrationContext _context;

    public UserRepository(IntegrationContext context)
    {
        _context = context;
    }
    
    public Task<List<UserEntity>> GetAllUsersAsync()
    {
        return _context.Users.ToListAsync();
    }

    public Task<UserEntity?> Find(int userId)
    {
        return _context.Users.FindAsync(userId).AsTask();
    }
    
    public Task AddRangeAsync(IEnumerable<UserEntity> entities)
    {
        _context.Users.AddRange(entities);
         return _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(UserEntity userEntity)
    {
        _context.Users.Remove(userEntity);
        await _context.SaveChangesAsync();
    }
}