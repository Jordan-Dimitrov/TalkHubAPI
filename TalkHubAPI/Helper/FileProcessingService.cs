using Microsoft.AspNetCore.Mvc;
using TalkHubAPI.Interfaces;
using System.Diagnostics;
using FFMpegCore;
using FFMpegCore.Enums;
namespace TalkHubAPI.Helper
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly string _UploadsDirectory;
        private readonly IBackgroundQueue _BackgroundQueue;
        public FileProcessingService(IBackgroundQueue backgroundQueue)
        {
            _BackgroundQueue = backgroundQueue;

            _UploadsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media");
            if (!Directory.Exists(_UploadsDirectory))
            {
                Directory.CreateDirectory(_UploadsDirectory);
            }
        }
        public async Task<FileContentResult> GetImageAsync(string fileName)
        {
            string filePath = Path.Combine(_UploadsDirectory, fileName);

            try
            {
                if (File.Exists(filePath))
                {
                    string contentType = GetContentType(fileName);
                    byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
                    return new FileContentResult(fileBytes, contentType);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        public string GetContentType(string fileName)
        {
            if (fileName.EndsWith(".webp", StringComparison.OrdinalIgnoreCase))
            {
                return "image/webp";
            }
            else if (fileName.EndsWith(".webm", StringComparison.OrdinalIgnoreCase))
            {
                return "video/webm";
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
            else
            {
                return "unsupported";
            }
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            if (file == null || file.Length == 0)
            {
                return "Empty";
            }

            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            if (fileExtension != ".jpg" && fileExtension != ".png" && fileExtension != ".webp")
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

            if (fileExtension == ".webp")
            {
                return webPFileName;
            }

            _BackgroundQueue.QueueTask(async token =>
            {
                await ConvertToWebp(filePath);
            });

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
        private async Task<bool> ConvertToWebm(string inputPath)
        {
            string outputPath = Path.Combine(_UploadsDirectory, Path.GetFileNameWithoutExtension(inputPath) + ".webm");

            await FFMpegArguments
               .FromFileInput(inputPath)
               .OutputToFile(outputPath, false, options =>
               options.WithVideoCodec(VideoCodec.LibVpx)
               .ForceFormat("webm")
               .WithConstantRateFactor(21)
               .WithAudioCodec(AudioCodec.LibVorbis)
               .WithVariableBitrate(4)
               .WithVideoFilters(filterOptions => filterOptions.Scale(VideoSize.Ld))
               .WithFastStart())
               .ProcessAsynchronously();

            await RemoveMediaAsync(inputPath);

            return true;
        }
        private async Task<bool> ConvertToWebp(string inputPath)
        {
            string outputPath = Path.Combine(_UploadsDirectory, Path.GetFileNameWithoutExtension(inputPath) + ".webp");

            await FFMpegArguments
               .FromFileInput(inputPath)
               .OutputToFile(outputPath, false, options =>
               options.ForceFormat("webp")
               .WithFastStart())
               .ProcessAsynchronously();

            await RemoveMediaAsync(inputPath);

            return true;
        }

        public async Task<string> UploadVideoAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return "Empty";
            }

            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
            string webmFileName = Path.GetFileNameWithoutExtension(fileName) + ".webm";
            string webmVideoPath = Path.Combine(_UploadsDirectory, webmFileName);

            if (fileExtension != ".mp4" && fileExtension != ".webm")
            {
                return "Invalid file format";
            }

            string filePath = Path.Combine(_UploadsDirectory, fileName);

            if (File.Exists(filePath) || File.Exists(webmVideoPath))
            {
                return "File already exists";
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            if(fileExtension == ".webm")
            {
                return webmFileName;
            }

            _BackgroundQueue.QueueTask(async token =>
            {
                await ConvertToWebm(filePath);
            });

            return webmFileName;
        }

        public FileStreamResult GetVideo(string fileName)
        {
            string filePath = Path.Combine(_UploadsDirectory, fileName);

            try
            {
                if (File.Exists(filePath))
                {
                    FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                    string contentType = GetContentType(fileName);

                    return new FileStreamResult(stream, contentType);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
