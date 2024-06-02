namespace Domain.DTOs.RoleDTOs;

public class GetRoleDto
{
    public int Id { get; set; }
    public required  string Name { get; set; } 
    public DateTimeOffset CreateAt { get; set; }
    public DateTimeOffset UpdateAt { get; set; }
}