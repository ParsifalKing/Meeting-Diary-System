using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Permissions;
using Infrastructure.Seed;

// using Infrastructure.Seed;
using Infrastructure.Services.AccountService;
using Infrastructure.Services.EmailService;
using Infrastructure.Services.FileService;
using Infrastructure.Services.HashService;
using Infrastructure.Services.MeetingService;
using Infrastructure.Services.NotificationService;
using Infrastructure.Services.RoleService;
using Infrastructure.Services.UserRoleService;
using Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace WebApi.ExtensionMethods.RegisterService;

public static class RegisterService
{
    public static void AddRegisterService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<DataContext>(configure =>
            configure.UseNpgsql(configuration.GetConnectionString("Connection")));

        services.AddScoped<Seeder>();
        services.AddScoped<IHashService, HashService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IMeetingService, MeetingService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

    }
}