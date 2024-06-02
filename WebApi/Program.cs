using Domain.DTOs.EmailDTOs;
using Infrastructure.Data;
using Infrastructure.Seed;

using Microsoft.EntityFrameworkCore;
using Serilog;
using WebApi.ExtensionMethods.AccountConfig;
using WebApi.ExtensionMethods.RegisterService;
using WebApi.ExtensionMethods.SwaggerConfig;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();


var emailConfig = builder.Configuration
    .GetSection("EmailConfiguration")
    .Get<EmailConfiguration>();

builder.Services.AddSingleton(emailConfig!);

builder.Services.AddRegisterService(builder.Configuration);

builder.Services.SwaggerService();

builder.Services.AddAuthConfigureService(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

try
{
    var serviceProvider = app.Services.CreateScope().ServiceProvider;
    var dataContext = serviceProvider.GetRequiredService<DataContext>();
    await dataContext.Database.MigrateAsync();

    var seeder = serviceProvider.GetRequiredService<Seeder>();
    await seeder.Initial();

}
catch (Exception)
{
}

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
