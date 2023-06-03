using Integration.Data.Models;
using Integration.Data.Repositories;
using Integration.Services;
using Integration.Services.Impl;
using Integration.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Integration.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPut("fetch")]
    public async Task<ActionResult> FetchData()
    {
        return Ok();
    }

    [HttpGet]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        return await _userService.GetAllAsync();
    }
    
    [HttpPut]
    public async Task<ActionResult> CreateUser([FromBody] User user)
    {
        return Ok();
    }
    
    [HttpPost]
    public async Task<ActionResult> UpdateUser([FromBody] User user)
    {
        return Ok();
    }
}