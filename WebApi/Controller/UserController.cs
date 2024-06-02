using System.Net;
using Domain.Constants;
using Domain.DTOs.UserDTOs;
using Domain.Filters;
using Infrastructure.Permissions;
using Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase

{
    [HttpGet("users")]
    [PermissionAuthorize(Permissions.User.View)]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilter filter)
    {
        var res1 = await userService.GetUsersAsync(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("{userId:int}")]
    [PermissionAuthorize(Permissions.User.View)]
    public async Task<IActionResult> GetUserById(int userId)
    {
        var res1 = await userService.GetUserByIdAsync(userId);
        return StatusCode(res1.StatusCode, res1);
    }


    [HttpPut("update")]
    [PermissionAuthorize(Permissions.User.Edit)]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto updateUser)
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value);
        var result = await userService.UpdateUserAsync(updateUser, userId);
        return StatusCode(result.StatusCode, result);
    }
}