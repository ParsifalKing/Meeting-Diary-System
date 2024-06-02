using System.Net;
using Domain.DTOs.MeetingDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.MeetingService;

public class MeetingService(ILogger<MeetingService> logger, DataContext context) : IMeetingService
{
    #region GetMeetingsAsync

    public async Task<PagedResponse<List<GetMeetingDto>>> GetMeetingsAsync(MeetingFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetMeetingsAsync} in time:{DateTime} ", "GetMeetingsAsync",
                DateTimeOffset.UtcNow);
            var Meetings = context.Meetings.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
                Meetings = Meetings.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));

            var response = await Meetings.Select(x => new GetMeetingDto()
            {
                Name = x.Name,
                Description = x.Description,
                EndDate = x.EndDate,
                StartDate = x.StartDate,
                UserId = x.UserId,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await Meetings.CountAsync();

            logger.LogInformation("Finished method {GetMeetingsAsync} in time:{DateTime} ", "GetMeetingsAsync",
                DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetMeetingDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetMeetingDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetMeetingByIdAsync

    public async Task<Response<GetMeetingDto>> GetMeetingByIdAsync(int MeetingId)
    {
        try
        {
            logger.LogInformation("Starting method {GetMeetingByIdAsync} in time:{DateTime} ", "GetMeetingByIdAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Meetings.Select(x => new GetMeetingDto()
            {
                Name = x.Name,
                Description = x.Description,
                EndDate = x.EndDate,
                StartDate = x.StartDate,
                UserId = x.UserId,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                Id = x.Id,
            }).FirstOrDefaultAsync(x => x.Id == MeetingId);

            if (existing is null)
            {
                logger.LogWarning("Could not find Meeting with Id:{Id},time:{DateTimeNow}", MeetingId, DateTimeOffset.UtcNow);
                return new Response<GetMeetingDto>(HttpStatusCode.BadRequest, $"Not found Meeting by id:{MeetingId}");
            }


            logger.LogInformation("Finished method {GetMeetingByIdAsync} in time:{DateTime} ", "GetMeetingByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetMeetingDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetMeetingDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateMeetingAsync

    public async Task<Response<string>> CreateMeetingAsync(CreateMeetingDto createMeeting)
    {
        try
        {
            logger.LogInformation("Starting method {CreateMeetingAsync} in time:{DateTime} ", "CreateMeetingAsync",
                DateTimeOffset.UtcNow);

            var newMeeting = new Meeting()
            {
                Name = createMeeting.Name,
                Description = createMeeting.Description,
                StartDate = createMeeting.StartDate,
                EndDate = createMeeting.EndDate,
                UserId = createMeeting.UserId,
                CreateAt = DateTimeOffset.UtcNow,
                UpdateAt = DateTimeOffset.UtcNow,
            };

            await context.Meetings.AddAsync(newMeeting);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {CreateMeetingAsync} in time:{DateTime} ", "CreateMeetingAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created Meeting by Id:{newMeeting.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateMeetingAsync

    public async Task<Response<string>> UpdateMeetingAsync(UpdateMeetingDto updateMeeting)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateMeetingAsync} in time:{DateTime} ", "UpdateMeetingAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Meetings.FirstOrDefaultAsync(x => x.Id == updateMeeting.Id);
            if (existing == null)
            {
                logger.LogWarning("Meeting not found by id:{Id},time:{Time}", updateMeeting.Id, DateTimeOffset.UtcNow);
                new Response<string>(HttpStatusCode.BadRequest, $"Not found Booking by id:{updateMeeting.Id}");
            }

            existing!.Name = updateMeeting.Name;
            existing.Description = updateMeeting.Description;
            existing.StartDate = updateMeeting.StartDate;
            existing.EndDate = updateMeeting.EndDate;
            existing.UserId = updateMeeting.UserId;
            existing.UpdateAt = DateTimeOffset.UtcNow;

            logger.LogInformation("Finished method {UpdateMeetingAsync} in time:{DateTime} ", "UpdateMeetingAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated Meeting by id:{updateMeeting.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteMeetingAsync

    public async Task<Response<bool>> DeleteMeetingAsync(int MeetingId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteMeetingAsync} in time:{DateTime} ", "DeleteMeetingAsync",
                DateTimeOffset.UtcNow);

            var Meeting = await context.Meetings.Where(x => x.Id == MeetingId).ExecuteDeleteAsync();

            logger.LogInformation("Finished method {DeleteMeetingAsync} in time:{DateTime} ", "DeleteMeetingAsync",
                DateTimeOffset.UtcNow);
            return Meeting == 0
                ? new Response<bool>(HttpStatusCode.BadRequest, $"Meeting not found by id:{MeetingId}")
                : new Response<bool>(true);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion


    #region GetUpcomingMeetings

    public async Task<PagedResponse<List<GetMeetingDto>>> GetUpcomingMeetings(PaginationFilter filter, int userId)
    {
        try
        {
            logger.LogInformation("Starting method {GetMeetingsAsync} in time:{DateTime} ", "GetMeetingsAsync",
                DateTimeOffset.UtcNow);
            var Meetings = context.Meetings.AsQueryable();

            var response = await Meetings.Select(x => new GetMeetingDto()
            {
                Name = x.Name,
                Description = x.Description,
                EndDate = x.EndDate,
                StartDate = x.StartDate,
                UserId = x.UserId,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).Where(x => x.UserId == userId && x.StartDate > DateTime.UtcNow).ToListAsync();

            var totalRecord = await Meetings.CountAsync();

            logger.LogInformation("Finished method {GetMeetingsAsync} in time:{DateTime} ", "GetMeetingsAsync",
                DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetMeetingDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetMeetingDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
}
