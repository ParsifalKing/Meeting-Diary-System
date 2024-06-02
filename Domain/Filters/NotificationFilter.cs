namespace Domain.Filters;

public class NotificationFilter : PaginationFilter
{
    public DateTime? SendDate { get; set; }
}
