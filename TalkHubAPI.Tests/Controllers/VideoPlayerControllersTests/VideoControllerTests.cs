using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using TalkHubAPI.Controllers.VideoPlayerControllers;
using TalkHubAPI.Dtos.VideoPlayerDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Tests.Controller.VideoPlayerControllersTests
{
    public class VideoControllerTests
    {
        private readonly IVideoRepository _VideoRepository;
        private readonly IMapper _Mapper;
        private readonly IAuthService _AuthService;
        private readonly IUserRepository _UserRepository;
        private readonly IFileProcessingService _FileProcessingService;
        private readonly IVideoUserLikeRepository _VideoUserLikeRepository;
        private readonly IVideoTagRepository _VideoTagRepository;
        private readonly IPlaylistRepository _PlaylistRepository;
        private readonly IBackgroundQueue _BackgroundQueue;
        private readonly IUserSubscribtionRepository _UserSubscribtionRepository;
        public VideoControllerTests()
        {
            _VideoRepository = A.Fake<IVideoRepository>();
            _Mapper = A.Fake<IMapper>();
            _AuthService = A.Fake<IAuthService>();
            _UserRepository = A.Fake<IUserRepository>();
            _FileProcessingService = A.Fake<IFileProcessingService>();
            _VideoUserLikeRepository = A.Fake<IVideoUserLikeRepository>();
            _VideoTagRepository = A.Fake<IVideoTagRepository>();
            _PlaylistRepository = A.Fake<IPlaylistRepository>();
            _BackgroundQueue = A.Fake<IBackgroundQueue>();
            _UserSubscribtionRepository = A.Fake<IUserSubscribtionRepository>();
        }

        [Fact]
        public async Task VideoController_GetVideo_ReturnOk()
        {
            int videoId = 1;
            VideoDto videoDto = A.Fake<VideoDto>();
            Video video = A.Fake<Video>();

            A.CallTo(() => _VideoRepository.GetVideoAsync(videoId))
                .Returns(video);
            VideoController controller = new VideoController(_VideoRepository,
                _Mapper, _AuthService,
                _UserRepository, _FileProcessingService,
                _VideoUserLikeRepository, _VideoTagRepository,
                _PlaylistRepository, _BackgroundQueue, _UserSubscribtionRepository);

            IActionResult result = await controller.GetVideo(videoId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
