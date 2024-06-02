using Domain.Constants;
using Domain.DTOs.RoleDTOs;
using Domain.Filters;
using Infrastructure.Permissions;
using Infrastructure.Services.RoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class RoleController(IRoleService roleService) : ControllerBase
{
    [HttpGet("roles")]
    [PermissionAuthorize(Permissions.Role.View)]
    public async Task<IActionResult> GetRoles([FromQuery] RoleFilter filter)
    {
        var response = await roleService.GetRolesAsync(filter);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{roleId:int}")]
    [PermissionAuthorize(Permissions.Role.View)]
    public async Task<IActionResult> GetRoleById(int roleId)
    {
        var response = await roleService.GetRoleByIdAsync(roleId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("create")]
    [PermissionAuthorize(Permissions.Role.Create)]
    public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRole)
    {
        var result = await roleService.CreateRoleAsync(createRole);
        return StatusCode(result.StatusCode, result);
    }


    [HttpPut("update")]
    [PermissionAuthorize(Permissions.Role.Edit)]
    public async Task<IActionResult> UpdateRole([FromBody] UpdateRoleDto updateRole)
    {
        var result = await roleService.UpdateRoleAsync(updateRole);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{roleId:int}")]
    [PermissionAuthorize(Permissions.Role.Delete)]
    public async Task<IActionResult> DeleteRole(int roleId)
    {
        var result = await roleService.DeleteRoleAsync(roleId);
        return StatusCode(result.StatusCode, result);
    }
}