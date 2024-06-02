using Domain.DTOs.RoleDTOs;
using Domain.DTOs.UserDTOs;

namespace Domain.DTOs.UserRoleDTOs;

public class GetUserRoleDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int RoleId { get; set; }
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdateAt { get; set; }
    public GetRoleDto? Role { get; set; }
    public GetUserDto? User { get; set; }
}