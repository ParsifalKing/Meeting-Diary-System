using Domain.Constants;
using Domain.DTOs.MeetingDTOs;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Permissions;
using Infrastructure.Services.MeetingService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

public class MeetingController(IMeetingService _meetingService) : ControllerBase
{
    [HttpGet("Meetings")]
    [PermissionAuthorize(Permissions.Meeting.View)]
    public async Task<IActionResult> GetMeetings([FromQuery] MeetingFilter filter)
    {
        var response = await _meetingService.GetMeetingsAsync(filter);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("GetMeetingBy{MeetingId:int}")]
    [PermissionAuthorize(Permissions.Meeting.View)]
    public async Task<IActionResult> GetMeetingById(int MeetingId)
    {
        var response = await _meetingService.GetMeetingByIdAsync(MeetingId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("Get-Upcoming-Meetings")]
    [PermissionAuthorize(Permissions.Meeting.View)]
    public async Task<IActionResult> GetUpcomingMeetings(PaginationFilter filter)
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value);
        var result = await _meetingService.GetUpcomingMeetings(filter, userId);
        return StatusCode(result.StatusCode, result);
    }

    [HttpPost("create-meeting")]
    [PermissionAuthorize(Permissions.Meeting.Create)]
    public async Task<IActionResult> CreateMeeting([FromBody] CreateMeetingDto createMeeting)
    {
        var result = await _meetingService.CreateMeetingAsync(createMeeting);
        return StatusCode(result.StatusCode, result);
    }


    [HttpPut("update")]
    [PermissionAuthorize(Permissions.Meeting.Edit)]
    public async Task<IActionResult> UpdateMeeting([FromBody] UpdateMeetingDto updateMeeting)
    {
        var result = await _meetingService.UpdateMeetingAsync(updateMeeting);
        return StatusCode(result.StatusCode, result);
    }

    [HttpDelete("{MeetingId:int}")]
    [PermissionAuthorize(Permissions.Meeting.Delete)]
    public async Task<IActionResult> DeleteMeeting(int MeetingId)
    {
        var result = await _meetingService.DeleteMeetingAsync(MeetingId);
        return StatusCode(result.StatusCode, result);
    }


}