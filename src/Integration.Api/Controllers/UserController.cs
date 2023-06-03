using Integration.Data.Models;
using Integration.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Integration.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPut("fetch")]
    public async Task<ActionResult> FetchData()
    {
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<UserEntity>>> GetAll()
    {
        return await _userRepository.GetAllUsersAsync();
    }
    
    [HttpPut]
    public async Task<ActionResult> CreateUser([FromBody] UserEntity userEntity)
    {
        return Ok();
    }
    
    [HttpPost]
    public async Task<ActionResult> UpdateUser([FromBody] UserEntity userEntity)
    {
        return Ok();
    }
}