using System.Net;
using Domain.DTOs.RoleDTOs;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.RoleService;

public class RoleService(ILogger<RoleService> logger, DataContext context) : IRoleService
{
    #region GetRolesAsync

    public async Task<PagedResponse<List<GetRoleDto>>> GetRolesAsync(RoleFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetRolesAsync} in time:{DateTime} ", "GetRolesAsync",
                DateTimeOffset.UtcNow);
            var roles = context.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(filter.RoleName))
                roles = roles.Where(x => x.Name.ToLower().Contains(filter.RoleName.ToLower()));

            var response = await roles.Select(x => new GetRoleDto()
            {
                Name = x.Name,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                Id = x.Id,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await roles.CountAsync();

            logger.LogInformation("Finished method {GetRolesAsync} in time:{DateTime} ", "GetRolesAsync",
                DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetRoleDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetRoleDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region GetRoleByIdAsync

    public async Task<Response<GetRoleDto>> GetRoleByIdAsync(int roleId)
    {
        try
        {
            logger.LogInformation("Starting method {GetRoleByIdAsync} in time:{DateTime} ", "GetRoleByIdAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Roles.Select(x => new GetRoleDto()
            {
                Name = x.Name,
                CreateAt = x.CreateAt,
                UpdateAt = x.UpdateAt,
                Id = x.Id
            }).FirstOrDefaultAsync(x => x.Id == roleId);

            if (existing is null)
            {
                logger.LogWarning("Could not find role with Id:{Id},time:{DateTimeNow}", roleId, DateTimeOffset.UtcNow);
                return new Response<GetRoleDto>(HttpStatusCode.BadRequest, $"Not found role by id:{roleId}");
            }


            logger.LogInformation("Finished method {GetRoleByIdAsync} in time:{DateTime} ", "GetRoleByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetRoleDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetRoleDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region CreateRoleAsync

    public async Task<Response<string>> CreateRoleAsync(CreateRoleDto createRole)
    {
        try
        {
            logger.LogInformation("Starting method {CreateRoleAsync} in time:{DateTime} ", "CreateRoleAsync",
                DateTimeOffset.UtcNow);
            var existingRole = await context.Roles.AnyAsync(x => x.Name == createRole.Name);
            if (existingRole)
            {
                logger.LogWarning("Role already exists by name:{Name},time:{Time}", createRole.Name,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest,
                    $"Already exists role by name:{createRole.Name}");
            }

            var newRole = new Role()
            {
                Name = createRole.Name,
                CreateAt = DateTimeOffset.UtcNow,
                UpdateAt = DateTimeOffset.UtcNow,
            };
            await context.Roles.AddAsync(newRole);
            await context.SaveChangesAsync();

            logger.LogInformation("Finished method {CreateRoleAsync} in time:{DateTime} ", "CreateRoleAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully created role by Id:{newRole.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region UpdateRoleAsync

    public async Task<Response<string>> UpdateRoleAsync(UpdateRoleDto updateRole)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateRoleAsync} in time:{DateTime} ", "UpdateRoleAsync",
                DateTimeOffset.UtcNow);

            var existing = await context.Roles.Where(x => x.Id == updateRole.Id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(r => r.Name, updateRole.Name)
                    .SetProperty(r => r.UpdateAt, DateTimeOffset.UtcNow));

            logger.LogInformation("Finished method {UpdateRoleAsync} in time:{DateTime} ", "UpdateRoleAsync",
                DateTimeOffset.UtcNow);

            return existing == 0
                ? new Response<string>(HttpStatusCode.BadRequest, $"Not found role by id:{updateRole.Id}")
                : new Response<string>($"Successfully updated role by id:{updateRole.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion

    #region DeleteRoleAsync

    public async Task<Response<bool>> DeleteRoleAsync(int roleId)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteRoleAsync} in time:{DateTime} ", "DeleteRoleAsync",
                DateTimeOffset.UtcNow);

            var role = await context.Roles.Where(x => x.Id == roleId).ExecuteDeleteAsync();

            logger.LogInformation("Finished method {DeleteRoleAsync} in time:{DateTime} ", "DeleteRoleAsync",
                DateTimeOffset.UtcNow);
            return role == 0
                ? new Response<bool>(HttpStatusCode.BadRequest, $"Role not found by id:{roleId}")
                : new Response<bool>(true);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    #endregion
}