using Domain.DTOs.MeetingDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.MeetingService;

public interface IMeetingService
{
    Task<PagedResponse<List<GetMeetingDto>>> GetMeetingsAsync(MeetingFilter filter);
    Task<Response<GetMeetingDto>> GetMeetingByIdAsync(int MeetingId);
    Task<Response<string>> CreateMeetingAsync(CreateMeetingDto createMeeting);
    Task<Response<string>> UpdateMeetingAsync(UpdateMeetingDto updateMeeting);
    Task<Response<bool>> DeleteMeetingAsync(int MeetingId);
    Task<PagedResponse<List<GetMeetingDto>>> GetUpcomingMeetings(PaginationFilter filter, int userId);
}
