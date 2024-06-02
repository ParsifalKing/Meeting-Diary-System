namespace Domain.DTOs.NotificationDTOs;

public class CreateNotificationDto
{
    public int MeetingId { get; set; }
    public int UserId { get; set; }
    public required string Massege { get; set; }
}
