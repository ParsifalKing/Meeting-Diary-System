using Domain.Enums;

namespace Domain.Filters;

public class UserFilter:PaginationFilter
{
    public string? Username { get; set; }
    public string? Email { get; set; } 
    public string? Phone { get; set; } 
    public Status? Status { get; set; }
}