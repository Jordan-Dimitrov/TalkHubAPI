using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TalkHubAPI.Controllers.VideoPlayerControllers;
using TalkHubAPI.Dtos.VideoPlayerDtos;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.ConfigurationModels;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Tests.Controller.VideoPlayerControllersTests
{
    public class VideoTagControllerTests
    {
        private readonly IVideoTagRepository _VideoTagRepository;
        private readonly IMemoryCache _MemoryCache;
        private readonly IMapper _Mapper;
        private readonly IOptions<MemoryCacheSettings> _MemoryCacheSettings;
        public VideoTagControllerTests()
        {
            _VideoTagRepository = A.Fake<IVideoTagRepository>();
            _MemoryCache = A.Fake<IMemoryCache>();
            _Mapper = A.Fake<IMapper>();
            _MemoryCacheSettings = A.Fake<IOptions<MemoryCacheSettings>>();
        }
        [Fact]
        public async Task VideoController_GetVideo_ReturnOk()
        {
            int videoTagId = 1;
            VideoTagDto videoTagDto = A.Fake<VideoTagDto>();
            VideoTag videoTag = A.Fake<VideoTag>();

            A.CallTo(() => _VideoTagRepository.GetVideoTagAsync(videoTagId))
                .Returns(videoTag);
            VideoTagController controller = new VideoTagController(_VideoTagRepository,
                _Mapper, _MemoryCache, _MemoryCacheSettings);

            IActionResult result = await controller.GetTag(videoTagId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
