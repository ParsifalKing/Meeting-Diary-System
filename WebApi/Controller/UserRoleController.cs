using Domain.Constants;
using Domain.DTOs.UserRoleDTOs;
using Domain.Filters;
using Infrastructure.Permissions;
using Infrastructure.Services.UserRoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserRoleController(IUserRoleService userUserRoleService) : ControllerBase
{
    [HttpGet("user-roles")]
    [PermissionAuthorize(Permissions.UserRole.View)]
    public async Task<IActionResult> GetUserUserRoles([FromQuery] PaginationFilter filter)
    {
        var res1 = await userUserRoleService.GetUserRolesAsync(filter);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpGet("get-by-id")]
    [PermissionAuthorize(Permissions.UserRole.View)]
    public async Task<IActionResult> GetUserRoleById([FromQuery] UserRoleDto userRole)
    {
        var res1 = await userUserRoleService.GetUserRoleByIdAsync(userRole);
        return StatusCode(res1.StatusCode, res1);
    }

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.UserRole.Create)]
    public async Task<IActionResult> CreateUserRole([FromBody] CreateUserRoleDto createUserRole)
    {
        var result = await userUserRoleService.CreateUserRoleAsync(createUserRole);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("delete")]
    [PermissionAuthorize(Permissions.UserRole.Delete)]
    public async Task<IActionResult> DeleteUserRole([FromBody] UserRoleDto userRole)
    {
        var result = await userUserRoleService.DeleteUserRoleAsync(userRole);
        return StatusCode(result.StatusCode, result);
    }
}