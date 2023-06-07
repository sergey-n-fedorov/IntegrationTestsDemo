using AutoMapper;
using Example.Data.Entities;
using Example.Data.Repositories;
using Example.Services.External;
using Example.Shared;
using Example.Shared.Context;
using Example.Shared.Models;

namespace Example.Services;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IExternalServiceClient _externalServiceClient;
    private readonly IExampleContextProvider _exampleContextProvider;

    public UserService(IUserRepository userRepository, IMapper mapper, IExternalServiceClient externalServiceClient, IExampleContextProvider exampleContextProvider)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _externalServiceClient = externalServiceClient;
        _exampleContextProvider = exampleContextProvider;
    }

    public async Task FetchAsync()
    {
        var externalUsers = await _externalServiceClient.GetAllUsersAsync();
        var dbUsersToAdd = _mapper.Map<List<UserEntity>>(externalUsers);
        await _userRepository.AddRangeAsync(dbUsersToAdd);
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        var dbUsers = await _userRepository.GetAllUsersAsync();
        return _mapper.Map<List<User>>(dbUsers);
    }
    
    public async Task DeleteAsync(int userId)
    {
        var dbUser = await _userRepository.Find(userId);

        if (dbUser != null)
        {
            await _externalServiceClient.DeleteUser(dbUser.ExternalId);
            await _userRepository.DeleteUserAsync(dbUser);
        }
    }
}