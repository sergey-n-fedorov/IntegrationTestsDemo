using Integration.Data.Entities;
using IntegrationService.Data;
using Microsoft.EntityFrameworkCore;

namespace Integration.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly IntegrationContext _context;

    public UserRepository(IntegrationContext context)
    {
        _context = context;
    }

    public Task AddUser(UserEntity userEntity)
    {
        _context.Users.Add(userEntity);
        return _context.SaveChangesAsync();
    }

    public Task<List<UserEntity>> GetAllUsersAsync()
    {
        return _context.Users.ToListAsync();
    }

    public Task<UserEntity?> Find(int userId)
    {
        return _context.Users.FindAsync(userId).AsTask();
    }
    
    public Task UpdateUserAsync(UserEntity userEntity)
    {
        _context.Entry(userEntity).State = EntityState.Modified;
        return _context.SaveChangesAsync();

    }

    public Task AddRangeAsync(IEnumerable<UserEntity> entities)
    {
        _context.Users.AddRange(entities);
         return _context.SaveChangesAsync();
    }
}