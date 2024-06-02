using System.Net;
using Domain.DTOs.RoleDTOs;
using Domain.DTOs.UserDTOs;
using Domain.DTOs.UserRoleDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.UserRoleService;

public class UserRoleService(DataContext context, ILogger<UserRoleService> logger) : IUserRoleService
{
    #region GetUserRolesAsync

    public async Task<PagedResponse<List<GetUserRoleDto>>> GetUserRolesAsync(PaginationFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetUserRolesAsync} in time:{DateTime} ", "GetUserRolesAsync",
                DateTimeOffset.UtcNow);

            var response = await context.UserRoles.Include(x => x.Role)
                .Include(x => x.User)
                .Select(x => new GetUserRoleDto()
                {
                    Role = new GetRoleDto()
                    {
                        Name = x.Role!.Name,
                        CreateAt = x.Role!.CreateAt,
                        UpdateAt = x.Role!.UpdateAt,
                        Id = x.Role!.Id
                    },
                    User = new GetUserDto()
                    {
                        Email = x.User!.Email,
                        Phone = x.User!.Phone,
                        Status = x.User.Status,
                        Username = x.User!.Username,
                        CreateAt = x.User!.CreateAt,
                        UpdateAt = x.User!.UpdateAt,
                        Id = x.User!.Id
                    },
                    UpdateAt = x.UpdateAt,
                    Id = x.Id,
                    CreateAt = x.CreateAt,
                    UserId = x.UserId,
                    RoleId = x.RoleId
                }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await context.UserRoles.CountAsync();
            logger.LogInformation("Finished method {GetUserRolesAsync} in time:{DateTime} ", "GetUserRolesAsync",
                DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetUserRoleDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetUserRoleDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetUserRoleByIdAsync

    public async Task<Response<GetUserRoleDto>> GetUserRoleByIdAsync(UserRoleDto userRoleDto)
    {
        try
        {
            logger.LogInformation("Starting method {GetUserRoleByIdAsync} in time:{DateTime} ", "GetUserRoleByIdAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.UserRoles.Include(x => x.Role)
                .Include(x => x.User)
                .Select(x => new GetUserRoleDto()
                {
                    Role = new GetRoleDto()
                    {
                        Name = x.Role!.Name,
                        CreateAt = x.Role!.CreateAt,
                        UpdateAt = x.Role!.UpdateAt,
                        Id = x.Role!.Id
                    },
                    User = new GetUserDto()
                    {
                        Email = x.User!.Email,
                        Phone = x.User!.Phone,
                        Status = x.User.Status,
                        Username = x.User!.Username,
                        CreateAt = x.User!.CreateAt,
                        UpdateAt = x.User!.UpdateAt,
                        Id = x.User!.Id
                    },
                    UpdateAt = x.UpdateAt,
                    Id = x.Id,
                    CreateAt = x.CreateAt,
                    UserId = x.UserId,
                    RoleId = x.RoleId
                }).FirstOrDefaultAsync(x => x.RoleId == userRoleDto.RoleId && x.UserId == userRoleDto.UserId);

            logger.LogInformation("Finished method {GetUserRoleByIdAsync} in time:{DateTime} ", "GetUserRoleByIdAsync",
                DateTimeOffset.UtcNow);
            return existing == null
                ? new Response<GetUserRoleDto>(HttpStatusCode.BadRequest,
                    $"UserRole not found by userId{userRoleDto.UserId},roleId={userRoleDto.RoleId}")
                : new Response<GetUserRoleDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<GetUserRoleDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateUserRoleAsync

    public async Task<Response<string>> CreateUserRoleAsync(CreateUserRoleDto createUserRole)
    {
        try
        {
            logger.LogInformation("Starting method {CreateUserRoleAsync} in time:{DateTime} ", "CreateUserRoleAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.UserRoles.AnyAsync(x =>
                x.RoleId == createUserRole.RoleId && x.UserId == createUserRole.UserId);
            if (existing)
            {
                logger.LogWarning("User Role already exists by id userId:{UserId},roleId:{RoleId},time:{DateTime}",
                    createUserRole.RoleId, createUserRole.UserId, DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "Already exists");
            }

            var newUserRole = new UserRole()
            {
                RoleId = createUserRole.RoleId,
                UserId = createUserRole.UserId,
                CreateAt = DateTimeOffset.UtcNow,
                UpdateAt = DateTimeOffset.UtcNow,
            };

            await context.UserRoles.AddAsync(newUserRole);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {CreateUserRoleAsync} in time:{DateTime} ", "CreateUserRoleAsync",
                DateTimeOffset.UtcNow);

            return new Response<string>($"Successfully created UserRole by id:{newUserRole.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteUserRoleAsync

    public async Task<Response<bool>> DeleteUserRoleAsync(UserRoleDto userRoleDto)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteUserRoleAsync} in time:{DateTime} ", "DeleteUserRoleAsync",
                DateTimeOffset.UtcNow);

            var userRole = await context.UserRoles.Where(x => x.RoleId == userRoleDto.RoleId && x.UserId == userRoleDto.UserId)
                .ExecuteDeleteAsync();

            logger.LogInformation("Finished method {DeleteUserRoleAsync} in time:{DateTime} ", "DeleteUserRoleAsync",
                DateTimeOffset.UtcNow);
            return userRole == 0
                ? new Response<bool>(HttpStatusCode.BadRequest, "UserRole not found")
                : new Response<bool>(true);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
}