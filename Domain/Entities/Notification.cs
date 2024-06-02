namespace Domain.Entities;

public class Notification : BaseEntity
{
    public int MeetingId { get; set; }
    public int UserId { get; set; }
    public string Massege { get; set; } = null!;
    public DateTime SendDate { get; set; }

    public Meeting? Meeting { get; set; }
    public User? User { get; set; }
}
