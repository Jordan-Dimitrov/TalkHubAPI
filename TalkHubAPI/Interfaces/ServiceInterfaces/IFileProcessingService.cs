using Microsoft.AspNetCore.Mvc;
using TalkHubAPI.Models;

namespace TalkHubAPI.Interfaces.ServiceInterfaces
{
    public interface IFileProcessingService
    {
        Task<string> UploadImageAsync(IFormFile file);
        Task<VideoUploadResponse> UploadVideoAsync(IFormFile file);
        Task<FileContentResult> GetImageAsync(string fileName);
        FileStreamResult GetVideo(string fileName);
        string GetContentType(string fileName);
        Task<bool> RemoveMediaAsync(string fileName);
        bool ImageMimeTypeValid(IFormFile file);
        bool VideoMimeTypeValid(IFormFile file);
    }
}
