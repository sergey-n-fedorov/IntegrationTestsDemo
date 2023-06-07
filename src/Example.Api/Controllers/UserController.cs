using Example.Services;
using Example.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Example.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("fetch")]
    public async Task<ActionResult> Fetch()
    {
        await _userService.FetchAsync();
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        return await _userService.GetAllAsync();
    }
    
    [HttpDelete("{userId}")]
    public async Task<ActionResult> Delete(int userId)
    {
        await _userService.DeleteAsync(userId);
        return Ok();
    }
    
    [HttpPost]
    public async Task<ActionResult> UpdateUser([FromBody] User user)
    {
        await _userService.UpdateUserAsync(user);
        return Ok();
    }
}