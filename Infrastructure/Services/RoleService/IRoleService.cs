using Domain.DTOs.RoleDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.RoleService;

public interface IRoleService
{
    Task<PagedResponse<List<GetRoleDto>>> GetRolesAsync(RoleFilter filter);
    Task<Response<GetRoleDto>> GetRoleByIdAsync(int roleId);
    Task<Response<string>> CreateRoleAsync(CreateRoleDto createRole);
    Task<Response<string>> UpdateRoleAsync(UpdateRoleDto updateRole);
    Task<Response<bool>> DeleteRoleAsync(int roleId);
}