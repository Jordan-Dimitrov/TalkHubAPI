using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace TalkHubAPI.Interfaces
{
    public interface IFileProcessingService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<string> UploadVideoAsync(IFormFile file);
        Task<FileContentResult> GetImageAsync(string fileName);
        FileStreamResult GetVideo(string fileName);
        string GetContentType(string fileName);
        Task<bool> RemoveMediaAsync(string fileName);
    }
}
