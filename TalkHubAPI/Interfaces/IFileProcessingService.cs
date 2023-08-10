using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace TalkHubAPI.Interfaces
{
    public interface IFileProcessingService
    {
        string UploadMedia(IFormFile file);
        FileContentResult GetMedia(string name);
        string GetContentType(string fileName);
    }
}
