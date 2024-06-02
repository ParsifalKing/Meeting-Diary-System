namespace Domain.Entities;

public class Meeting : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UserId { get; set; }

    public User? User { get; set; }
    public List<Notification>? Notifications { get; set; }
}
