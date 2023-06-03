using Integration.Data.Models;
using IntegrationService.Data;
using Microsoft.EntityFrameworkCore;

namespace Integration.Data.Repositories;

public interface IUserRepository
{
    Task AddUser(UserEntity userEntity);
    Task<List<UserEntity>> GetAllUsersAsync();
}

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
}