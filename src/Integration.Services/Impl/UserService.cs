using AutoMapper;
using Integration.Data.Repositories;
using Integration.Shared.Models;

namespace Integration.Services.Impl;

public class UserService : IUserService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public Task FetchAsync()
    {
        return Task.CompletedTask;
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        var dbUsers = await _userRepository.GetAllUsersAsync();
        return _mapper.Map<List<User>>(dbUsers);
    }
}