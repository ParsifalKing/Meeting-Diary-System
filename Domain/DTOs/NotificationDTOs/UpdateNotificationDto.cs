namespace Domain.DTOs.NotificationDTOs;

public class UpdateNotificationDto
{
    public int Id { get; set; }
    public int MeetingId { get; set; }
    public int UserId { get; set; }
    public required string Massege { get; set; }
}