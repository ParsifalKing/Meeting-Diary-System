namespace Domain.DTOs.MeetingDTOs;

public class CreateMeetingDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UserId { get; set; }

}
