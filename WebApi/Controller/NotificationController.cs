using Domain.Constants;
using Domain.DTOs.NotificationDTOs;
using Domain.Filters;
using Infrastructure.Permissions;
using Infrastructure.Services.NotificationService;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controller;

public class NotificationController(INotificationService _notificationService) : ControllerBase
{
    [HttpGet("Notifications")]
    [PermissionAuthorize(Permissions.Notification.View)]
    public async Task<IActionResult> GetNotifications([FromQuery] NotificationFilter filter)
    {
        var response = await _notificationService.GetNotificationsAsync(filter);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{NotificationId:int}")]
    [PermissionAuthorize(Permissions.Notification.View)]
    public async Task<IActionResult> GetNotificationById(int NotificationId)
    {
        var response = await _notificationService.GetNotificationByIdAsync(NotificationId);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("create-notification")]
    [PermissionAuthorize(Permissions.Notification.Create)]
    public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto createNotification)
    {
        var result = await _notificationService.CreateNotificationAsync(createNotification);
        return StatusCode(result.StatusCode, result);
    }


    [HttpPut("Send-Notifications")]
    [PermissionAuthorize(Permissions.Notification.Send)]
    public async Task<IActionResult> SendNotificationAsync()
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value);
        var result = await _notificationService.SendNotificationAsync(userId);
        return StatusCode(result.StatusCode, result);
    }
}
