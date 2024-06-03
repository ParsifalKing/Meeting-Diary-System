using System.Net;
using Domain.DTOs.EmailDTOs;
using Domain.DTOs.NotificationDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.EmailService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MimeKit.Text;
using Org.BouncyCastle.Math.EC.Rfc7748;

namespace Infrastructure.Services.NotificationService;

public class NotificationService(ILogger<NotificationService> logger, DataContext context, IEmailService emailService) : INotificationService
{
    #region GetNotificationsAsync

    public async Task<PagedResponse<List<GetNotificationDto>>> GetNotificationsAsync(NotificationFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetNotificationsAsync} in time:{DateTime} ", "GetNotificationsAsync",
                DateTimeOffset.UtcNow);
            var Notifications = context.Notifications.AsQueryable();

            if (filter.SendDate != null)
                Notifications = Notifications.Where(x => x.SendDate >= filter.SendDate);

            var response = await Notifications.Select(x => new GetNotificationDto()
            {
                Massege = x.Massege,
                MeetingId = x.MeetingId,
                SendDate = x.SendDate,
                UserId = x.UserId,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await Notifications.CountAsync();

            logger.LogInformation("Finished method {GetNotificationsAsync} in time:{DateTime} ", "GetNotificationsAsync",
                DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetNotificationDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetNotificationDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetNotificationByIdAsync

    public async Task<Response<GetNotificationDto>> GetNotificationByIdAsync(int NotificationId)
    {
        try
        {
            logger.LogInformation("Starting method {GetNotificationByIdAsync} in time:{DateTime} ", "GetNotificationByIdAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Notifications.Select(x => new GetNotificationDto()
            {
                Massege = x.Massege,
                MeetingId = x.MeetingId,
                SendDate = x.SendDate,
                UserId = x.UserId,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                Id = x.Id,
            }).FirstOrDefaultAsync(x => x.Id == NotificationId);

            if (existing is null)
            {
                logger.LogWarning("Could not find Notification with Id:{Id},time:{DateTimeNow}", NotificationId, DateTimeOffset.UtcNow);
                return new Response<GetNotificationDto>(HttpStatusCode.BadRequest, $"Not found Notification by id:{NotificationId}");
            }


            logger.LogInformation("Finished method {GetNotificationByIdAsync} in time:{DateTime} ", "GetNotificationByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetNotificationDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetNotificationDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateNotificationAsync

    public async Task<Response<string>> CreateNotificationAsync(CreateNotificationDto createNotification)
    {
        try
        {
            logger.LogInformation("Starting method {CreateNotificationAsync} in time:{DateTime} ", "CreateNotificationAsync",
                DateTimeOffset.UtcNow);

            var newNotification = new Notification()
            {
                Massege = createNotification.Massege,
                MeetingId = createNotification.MeetingId,
                UserId = createNotification.UserId,
                SendDate = DateTime.UtcNow,
                CreateAt = DateTimeOffset.UtcNow,
                UpdateAt = DateTimeOffset.UtcNow,
            };

            await context.Notifications.AddAsync(newNotification);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {CreateNotificationAsync} in time:{DateTime} ", "CreateNotificationAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created Notification by Id:{newNotification.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion


    #region GetUpcomingNotifications

    public async Task<Response<string>> SendNotificationAsync(int userId)
    {
        try
        {
            logger.LogInformation("Starting method SendNotificationAsync in time {DateTime}", DateTime.UtcNow);
            var meeting = await context.Meetings.Where(m => m.StartDate > DateTime.UtcNow && userId == m.UserId).OrderBy(x => x.StartDate).FirstOrDefaultAsync();

            if (meeting == null)
            {
                logger.LogWarning("Meetings not found at time : {DateTime}", DateTime.UtcNow);
            }
            var meetingString = $" Meeting name : {meeting!.Name} \nMeeting description : {meeting.Description} \nMeeting start date : {meeting.StartDate} \nMeeting end date : {meeting.EndDate}";

            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
            if (user == null)
            {
                logger.LogWarning("User with id {UserId} not found , time={DateTimeNow}", userId, DateTimeOffset.UtcNow);
                return new PagedResponse<string>(HttpStatusCode.BadRequest, "User not found");
            }
            await emailService.SendEmail(new EmailMessageDto(new[] { user.Email }, "All information for first user meeting",
                $"<h1>{meetingString}</h1>"), TextFormat.Html);

            logger.LogInformation("Finished method SendNotificationAsync in time {DateTime}", DateTime.UtcNow);
            return new Response<string>("Notification Successfully sent!!! ");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
}

