using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace TalkHubAPI.Interfaces
{
    public interface IFileProcessingService
    {
        string UploadMedia(IFormFile file);
        FileContentResult GetMedia(string fileName);
        string GetContentType(string fileName);
        bool RemoveMedia(string fileName);
    }
}
