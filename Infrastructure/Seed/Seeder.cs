using Domain.Constants;
using Domain.DTOs.RolePermissionDTOs;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Infrastructure.Helpers;
using Infrastructure.Services.HashService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Seed;


public class Seeder(DataContext context, ILogger<Seeder> logger, IHashService hashService)
{
    public async Task Initial()
    {
        await SeedRole();
        await DefaultUsers();
        await SeedClaimsForSuperAdmin();
        await AddAdminPermissions();
        await AddUserPermissions();
    }


    #region SeedRole

    private async Task SeedRole()
    {
        try
        {
            var newRoles = new List<Role>()
            {
                new()
                {
                    Name = Roles.SuperAdmin,
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new()
                {
                    Name = Roles.Admin,
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
                new()
                {
                    Name = Roles.User,
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow
                },
            };

            var existing = await context.Roles.ToListAsync();
            foreach (var role in newRoles)
            {
                if (existing.Exists(e => e.Name == role.Name) == false)
                {
                    await context.Roles.AddAsync(role);
                }
            }

            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            //ignored
        }
    }

    #endregion


    #region DefaultUsers

    private async Task DefaultUsers()
    {
        try
        {
            //super-admin
            var existingSuperAdmin = await context.Users.FirstOrDefaultAsync(x => x.Username == "SuperAdmin");
            if (existingSuperAdmin is null)
            {
                var superAdmin = new User()
                {
                    Email = "superadmin@gmail.com",
                    Phone = "123456780",
                    Username = "SuperAdmin",
                    Status = "Active",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow,
                    Password = hashService.ConvertToHash("12345")
                };
                await context.Users.AddAsync(superAdmin);
                await context.SaveChangesAsync();

                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Username == "SuperAdmin");
                var existingRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.SuperAdmin);
                if (existingUser is not null && existingRole is not null)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = existingRole.Id,
                        UserId = existingUser.Id,
                        Role = existingRole,
                        User = existingUser,
                        UpdateAt = DateTimeOffset.UtcNow,
                        CreateAt = DateTimeOffset.UtcNow
                    };
                    await context.UserRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();
                }

            }


            //admin
            var existingAdmin = await context.Users.FirstOrDefaultAsync(x => x.Username == "Admin");
            if (existingAdmin is null)
            {
                var admin = new User()
                {
                    Email = "admin@gmail.com",
                    Phone = "123456780",
                    Username = "Admin",
                    Status = "Active",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow,
                    Password = hashService.ConvertToHash("1234")
                };
                await context.Users.AddAsync(admin);
                await context.SaveChangesAsync();

                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Username == "Admin");
                var existingRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.Admin);
                if (existingUser is not null && existingRole is not null)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = existingRole.Id,
                        UserId = existingUser.Id,
                        Role = existingRole,
                        User = existingUser,
                        UpdateAt = DateTimeOffset.UtcNow,
                        CreateAt = DateTimeOffset.UtcNow
                    };
                    await context.UserRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();
                }

            }

            //User
            var User = await context.Users.FirstOrDefaultAsync(x => x.Username == "User");
            if (User is null)
            {
                var newUser = new User()
                {
                    Email = "user@gmail.com",
                    Phone = "123456789",
                    Username = "User",
                    Status = "Active",
                    CreateAt = DateTimeOffset.UtcNow,
                    UpdateAt = DateTimeOffset.UtcNow,
                    Password = hashService.ConvertToHash("123")
                };
                await context.Users.AddAsync(newUser);
                await context.SaveChangesAsync();

                var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Username == "User");
                var existingRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.User);
                if (existingUser is not null && existingRole is not null)
                {
                    var userRole = new UserRole()
                    {
                        RoleId = existingRole.Id,
                        UserId = existingUser.Id,
                        Role = existingRole,
                        User = existingUser,
                        UpdateAt = DateTimeOffset.UtcNow,
                        CreateAt = DateTimeOffset.UtcNow
                    };
                    await context.UserRoles.AddAsync(userRole);
                    await context.SaveChangesAsync();
                }

            }

        }
        catch (Exception e)
        {
            //ignored;
        }
    }

    #endregion


    #region SeedClaimsForSuperAdmin

    private async Task SeedClaimsForSuperAdmin()
    {
        try
        {
            var superAdminRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.SuperAdmin);
            if (superAdminRole == null) return;
            var roleClaims = new List<RoleClaimsDto>();
            roleClaims.GetPermissions(typeof(Domain.Constants.Permissions));
            var existingClaims = await context.RoleClaims.Where(x => x.RoleId == superAdminRole.Id).ToListAsync();
            foreach (var claim in roleClaims)
            {
                if (existingClaims.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value) == false)
                    await context.AddPermissionClaim(superAdminRole, claim.Value);
            }
        }
        catch (Exception ex)
        {
            // ignored
        }
    }

    #endregion

    #region AddAdminPermissions

    private async Task AddAdminPermissions()
    {
        //add claims
        var adminRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.Admin);
        if (adminRole == null) return;
        var userClaims = new List<RoleClaimsDto>()
        {
            new("Permissions", Domain.Constants.Permissions.Notification.View),
            new("Permissions", Domain.Constants.Permissions.Notification.Create),
            new("Permissions", Domain.Constants.Permissions.Notification.Send),

            new("Permissions", Domain.Constants.Permissions.Meeting.View),
            new("Permissions", Domain.Constants.Permissions.Meeting.Create),
            new("Permissions", Domain.Constants.Permissions.Meeting.Edit),

            new("Permissions", Domain.Constants.Permissions.Role.View),
            new("Permissions", Domain.Constants.Permissions.Role.Create),
            new("Permissions", Domain.Constants.Permissions.Role.Edit),

            new("Permissions", Domain.Constants.Permissions.User.View),
            new("Permissions", Domain.Constants.Permissions.User.Create),
            new("Permissions", Domain.Constants.Permissions.User.Edit),

            new("Permissions", Domain.Constants.Permissions.UserRole.View),
            new("Permissions", Domain.Constants.Permissions.UserRole.Create),

        };

        var existingClaim = await context.RoleClaims.Where(x => x.RoleId == adminRole.Id).ToListAsync();
        foreach (var claim in userClaims)
        {
            if (!existingClaim.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
            {
                await context.AddPermissionClaim(adminRole, claim.Value);
            }
        }
    }

    #endregion

    #region AddUserPermissions

    private async Task AddUserPermissions()
    {
        //add claims
        var userRole = await context.Roles.FirstOrDefaultAsync(x => x.Name == Roles.User);
        if (userRole == null) return;
        var userClaims = new List<RoleClaimsDto>()
        {
            new("Permissions", Domain.Constants.Permissions.Meeting.View),
            new("Permissions", Domain.Constants.Permissions.Notification.View),
            new("Permissions", Domain.Constants.Permissions.Role.View),
        };

        var existingClaim = await context.RoleClaims.Where(x => x.RoleId == userRole.Id).ToListAsync();
        foreach (var claim in userClaims)
        {
            if (!existingClaim.Any(x => x.ClaimType == claim.Type && x.ClaimValue == claim.Value))
            {
                await context.AddPermissionClaim(userRole, claim.Value);
            }
        }
    }

    #endregion


}




