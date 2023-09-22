using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkHubAPI.Interfaces;

namespace TalkHubAPI.Tests.Services
{
    public class FileProcessingServiceTests
    {
        private readonly IFileProcessingService _FileProcessingService;
        private readonly string _UploadsDirectory;
        public FileProcessingServiceTests()
        {
            _FileProcessingService = A.Fake<IFileProcessingService>();
            _UploadsDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media");
        }

        [Fact]
        public async Task FileProcessingService_GetImageAsync_ReturnsFileContentResult()
        {
            string fileName = "hidden.webp";
            string filePath = Path.Combine(_UploadsDirectory, fileName);

            Directory.CreateDirectory(_UploadsDirectory);

            File.WriteAllBytes(filePath, new byte[] { 0x1, 0x2, 0x3 });
            A.CallTo(() => _FileProcessingService.GetImageAsync(fileName))
               .Returns(new FileContentResult(new byte[] { 0x1, 0x2, 0x3 }, "image/webp"));

            FileContentResult result = await _FileProcessingService.GetImageAsync(fileName);

            Assert.NotNull(result);
            Assert.IsType<FileContentResult>(result);
            Assert.Equal("image/webp", result.ContentType);
        }
        //TODO: Add a test for every method
        
    }
}
