using Microsoft.AspNetCore.Mvc;
using TalkHubAPI.Interfaces;
using System.Diagnostics;
using FFMpegCore;
using FFMpegCore.Enums;
using System.Collections.Concurrent;
using TalkHubAPI.Models;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using Microsoft.Extensions.Options;
using TalkHubAPI.Models.ConfigurationModels;

namespace TalkHubAPI.Helper
{
    public class FileProcessingService : IFileProcessingService
    {
        private readonly string _UploadsDirectory;
        private readonly IBackgroundQueue _BackgroundQueue;
        private readonly IList<string> _SupportedImageMimeTypes;
        private readonly IList<string> _SupportedVideoMimeTypes;
        private readonly FFMpegConfig _FFMpegConfig;
        public FileProcessingService(IBackgroundQueue backgroundQueue, 
            IConfiguration configuration, 
            IOptions<FFMpegConfig> ffmpegConfigOptions)
        {
            _BackgroundQueue = backgroundQueue;
            _FFMpegConfig = ffmpegConfigOptions.Value;

            _SupportedImageMimeTypes = new List<string>() { "image/webp", "image/png", "image/jpg", "image/jpeg" };
            _SupportedVideoMimeTypes = new List<string>() { "video/webm", "video/mp4" };

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

            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

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

            await ConvertToWebp(filePath);

            return webPFileName;
        }

        public async Task<bool> RemoveMediaAsync(string fileName)
        {
            string filePath = Path.Combine(_UploadsDirectory, fileName);

            if (File.Exists(filePath))
            {
                await Task.Run(() => File.Delete(filePath));
                return true;
            }

            return false;
        }

        private async Task<bool> ConvertToWebm(string inputPath, Guid taskId)
        {
            string outputPath = Path.Combine(_UploadsDirectory, Path.GetFileNameWithoutExtension(inputPath) + ".webm");

            _BackgroundQueue.AddStatus(taskId, "In progress");

            var mediaInfo = await FFProbe.AnalyseAsync(inputPath);
            int bitRate = (int) mediaInfo.Format.BitRate / 2048;

            await FFMpegArguments
               .FromFileInput(inputPath)
               .OutputToFile(outputPath, false, options =>
               options.WithVideoCodec(VideoCodec.LibVpx)
               .ForceFormat("webm")
               .WithConstantRateFactor(18)
               .WithAudioCodec(AudioCodec.LibVorbis)
               .WithVariableBitrate(5)
               .WithFastStart()
               .WithoutMetadata()
               .WithVideoBitrate(bitRate)
               .UsingThreads(_FFMpegConfig.VideoConversionThreads))
               .ProcessAsynchronously();

            await RemoveMediaAsync(inputPath);

            _BackgroundQueue.AddStatus(taskId, "Finished");

            return true;
        }

        private async Task<bool> ConvertToWebp(string inputPath)
        {
            string outputPath = Path.Combine(_UploadsDirectory, Path.GetFileNameWithoutExtension(inputPath) + ".webp");

            await FFMpegArguments
               .FromFileInput(inputPath)
               .OutputToFile(outputPath, false, options =>
               options.ForceFormat("webp")
               .WithFastStart()
               .UsingThreads(_FFMpegConfig.PhotoConversionThreads))
               .ProcessAsynchronously();

            await RemoveMediaAsync(inputPath);

            return true;
        }

        public async Task<VideoUploadResponse> UploadVideoAsync(IFormFile file)
        {

            string fileName = Path.GetFileName(file.FileName);
            string fileExtension = Path.GetExtension(fileName).ToLowerInvariant();

            string webmFileName = Path.GetFileNameWithoutExtension(fileName) + ".webm";
            string webmVideoPath = Path.Combine(_UploadsDirectory, webmFileName);
            string filePath = Path.Combine(_UploadsDirectory, fileName);

            VideoUploadResponse videoUploadResponse = new VideoUploadResponse();

            if (File.Exists(filePath) || File.Exists(webmVideoPath))
            {
                videoUploadResponse.Error = "File already exists";
                return videoUploadResponse;
            }

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            videoUploadResponse.WebmFileName = webmFileName;

            if (fileExtension == ".webm")
            {
                return videoUploadResponse;
            }

            Guid taskId = Guid.NewGuid();

            _BackgroundQueue.QueueTask(async token =>
            {
                await ConvertToWebm(filePath, taskId);
            });

            videoUploadResponse.TaskId = taskId;

            return videoUploadResponse;
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

        public bool ImageMimeTypeValid(IFormFile file)
        {
            if(file is null || file.Length == 0)
            {
                return false;
            }

            if (!_SupportedImageMimeTypes.Contains(file.ContentType))
            {
                return false;
            }

            return true;
        }

        public bool VideoMimeTypeValid(IFormFile file)
        {
            if (file is null || file.Length == 0)
            {
                return false;
            }

            if (!_SupportedVideoMimeTypes.Contains(file.ContentType))
            {
                return false;
            }

            return true;
        }
    }
}
