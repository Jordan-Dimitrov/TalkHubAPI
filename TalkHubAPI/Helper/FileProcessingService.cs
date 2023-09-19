using ImageProcessor.Plugins.WebP.Imaging.Formats;
using ImageProcessor;
using Microsoft.AspNetCore.Mvc;
using TalkHubAPI.Interfaces;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace TalkHubAPI.Helper
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly string _UploadsDirectory;
        private readonly int _ImageQuality;
        public FileProcessingService()
        {
            _UploadsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media");
            _ImageQuality = 75;
        }
        public async Task<FileContentResult> GetImageAsync(string fileName)
        {
            string filePath = Path.Combine(_UploadsDirectory, fileName);

            if (File.Exists(filePath))
            {
                string contentType = GetContentType(fileName);
                byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                return new FileContentResult(fileBytes, contentType);
            }

            return null;
        }
        public string GetContentType(string fileName)
        {
            if (fileName.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
            {
                return "image/webp";
            }
            else if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase))
            {
                return "image/png";
            }
            else if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
            {
                return "image/jpg";
            }
            else if (fileName.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
            {
                return "video/mp4";
            }
            else if (fileName.EndsWith(".webm", StringComparison.OrdinalIgnoreCase))
            {
                return "video/webm";
            }
            else
            {
                return "unsupported";
            }
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "Empty";
            }

            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            if (fileExtension != ".jpg" && fileExtension != ".png")
            {
                return "Invalid file format";
            }

            string webPFileName = Path.GetFileNameWithoutExtension(file.FileName) + ".webp";
            string filePath = Path.Combine(_UploadsDirectory, fileName);
            string webPImagePath = Path.Combine(_UploadsDirectory, webPFileName);

            if (File.Exists(filePath) || File.Exists(webPImagePath))
            {
                return "File already exists";
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            using (FileStream webPFileStream = new FileStream(webPImagePath, FileMode.Create))
            {
                using (ImageFactory imageFactory = new ImageFactory(preserveExifData: false))
                {
                    imageFactory.Load(file.OpenReadStream())
                                .Format(new WebPFormat())
                                .Quality(_ImageQuality)
                                .Save(webPFileStream);
                }
            }

            File.Delete(filePath);

            return webPFileName;
        }

        public Task<bool> RemoveMediaAsync(string fileName)
        {
            string filePath = Path.Combine(_UploadsDirectory, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public async Task<string> UploadVideoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "Empty";
            }

            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            if (fileExtension != ".mp4")
            {
                return "Invalid file format. Only .mp4 files are supported.";
            }

            string filePath = Path.Combine(_UploadsDirectory, fileName);

            if (File.Exists(filePath))
            {
                return "File already exists";
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            MediaFile inputFile = new MediaFile { Filename = filePath };
            string webMFileName = Path.GetFileNameWithoutExtension(file.FileName) + ".webm";
            string webMFilePath = Path.Combine(_UploadsDirectory, webMFileName);
            MediaFile outputFile = new MediaFile { Filename = webMFilePath };

            using (Engine engine = new Engine())
            {
                engine.Convert(inputFile, outputFile);
            }

            File.Delete(filePath);

            return webMFileName;
        }

        public FileStreamResult GetVideos(string fileName)
        {
            string filePath = Path.Combine(_UploadsDirectory, fileName);

            if (!File.Exists(filePath))
            {
                return null;
            }

            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

            string contentType = GetContentType(fileName);

            return new FileStreamResult(stream, contentType);
        }
    }
}
