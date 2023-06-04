using AutoMapper;
using Integration.Data.Entities;
using Integration.Data.Repositories;
using Integration.Services.External;
using Integration.Shared.Models;

namespace Integration.Services.Impl;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly IExternalServiceClient _externalServiceClient;

    public UserService(IUserRepository userRepository, IMapper mapper, IExternalServiceClient externalServiceClient)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _externalServiceClient = externalServiceClient;
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

    public async Task UpdateUserAsync(User user)
    {
        var dbUser = await _userRepository.Find(user.Id);

        if (dbUser == null) throw new KeyNotFoundException();

        dbUser.Address = user.Address;
        dbUser.Name = user.Name;

        await _userRepository.UpdateUserAsync(dbUser);
    }
}