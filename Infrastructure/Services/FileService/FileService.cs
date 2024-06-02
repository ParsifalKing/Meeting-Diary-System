using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.FileService;

public class FileService(IWebHostEnvironment hostEnvironment, ILogger<FileService> logger) : IFileService
{
    #region CreateFile

    public async Task<string> CreateFile(IFormFile file)
    {
        try
        {
            logger.LogInformation("Starting method {CreateFile} in time:{DateTime} ", "CreateFile",
                DateTimeOffset.UtcNow);
            var fileName =
                string.Format($"{Guid.NewGuid() + Path.GetExtension(file.FileName)}");
            var fullPath = Path.Combine(hostEnvironment.WebRootPath, "images", fileName);
            await using var stream = new FileStream(fullPath, FileMode.Create);
            await file.CopyToAsync(stream);

            logger.LogInformation("Finished method {CreateFile} in time:{DateTime} ", "CreateFile",
                DateTimeOffset.UtcNow);
            return fileName;
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception} in time:{DateTime} ", e.Message, DateTimeOffset.UtcNow);
            return string.Empty;
        }
    }

    #endregion

    #region DeleteFile

    public bool DeleteFile(string file)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteFile} in time:{DateTime} ", "DeleteFile",
                DateTimeOffset.UtcNow);
            var fullPath = Path.Combine(hostEnvironment.WebRootPath, "images", file);
            File.Delete(fullPath);
            logger.LogInformation("Finished method {DeleteFile} in time:{DateTime} ", "DeleteFile",
                DateTimeOffset.UtcNow);
            return true;
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception} in time:{DateTime} ", e.Message, DateTimeOffset.UtcNow);
            return false;
        }
    }

    #endregion
}