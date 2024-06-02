using Domain.DTOs.UserDTOs;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.UserService;

public interface IUserService
{
    Task<PagedResponse<List<GetUserDto>>> GetUsersAsync(UserFilter filter);
    Task<Response<GetUserDto>> GetUserByIdAsync(int userId);
    Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUser,int userId);
}