using Microsoft.AspNetCore.Mvc;
using TalkHubAPI.Interfaces;

namespace TalkHubAPI.Helper
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly string _UploadsDirectory;
        public FileProcessingService()
        {
            _UploadsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media");
        }
        public FileContentResult GetMedia(string fileName)
        {
            string filePath = Path.Combine(_UploadsDirectory, fileName);

            if (File.Exists(filePath))
            {
                string contentType = GetContentType(fileName);
                byte[] fileBytes = File.ReadAllBytes(filePath);
                return new FileContentResult(fileBytes, contentType);
            }
            return null;
        }
        public string GetContentType(string fileName)
        {
            if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                return "image/png";
            }
            else if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                     fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return "image/jpeg";
            }
            else if (fileName.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
            {
                return "video/mp4";
            }
            else
            {
                return "unsupported";
            }
        }

        public string UploadMedia(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "Empty!";
            }

            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            if (fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png" && fileExtension != ".mp4")
            {
                return "Invalid file format. Only jpg, jpeg, png, and mp4 are supported.";
            }

            string filePath = Path.Combine(_UploadsDirectory, fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return fileName;
        }
    }
}
