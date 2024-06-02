using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace WebApi.ExtensionMethods.AccountConfig;

public static class AddAccountService
{
    public static IServiceCollection AddAuthConfigureService(this IServiceCollection services,
        IConfiguration configuration)
    {
        var key = configuration["JWT:Key"];

        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        return services;
    }
}