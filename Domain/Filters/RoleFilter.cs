namespace Domain.Filters;

public class RoleFilter:PaginationFilter
{
    public string? RoleName { get; set; }
}