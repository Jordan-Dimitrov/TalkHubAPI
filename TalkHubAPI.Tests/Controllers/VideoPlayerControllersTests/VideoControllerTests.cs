using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkHubAPI.Interfaces.VideoPlayerInterfaces;
using TalkHubAPI.Interfaces;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using TalkHubAPI.Controllers.VideoPlayerControllers;
using TalkHubAPI.Dtos.VideoPlayerDtos;
using TalkHubAPI.Models.VideoPlayerModels;
using TalkHubAPI.Repositories.VideoPlayerRepositories;
using FluentAssertions;

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
                _PlaylistRepository, _BackgroundQueue);

            IActionResult result = await controller.GetVideo(videoId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
