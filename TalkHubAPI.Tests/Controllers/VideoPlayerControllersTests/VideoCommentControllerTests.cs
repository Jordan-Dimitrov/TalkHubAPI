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
using TalkHubAPI.Interfaces.ServiceInterfaces;

namespace TalkHubAPI.Tests.Controller.VideoPlayerControllersTests
{
    public class VideoCommentControllerTests
    {
        private readonly IVideoCommentRepository _VideoCommentRepository;
        private readonly IMapper _Mapper;
        private readonly IUserRepository _UserRepository;
        private readonly IAuthService _AuthService;
        private readonly IVideoRepository _VideoRepository;
        private readonly IVideoCommentsLikeRepository _VideoCommentsLikeRepository;
        private readonly IVideoTagRepository _VideoTagRepository;
        public VideoCommentControllerTests()
        {
            _VideoCommentRepository = A.Fake<IVideoCommentRepository>();
            _Mapper = A.Fake<IMapper>();
            _UserRepository = A.Fake<IUserRepository>();
            _AuthService = A.Fake<IAuthService>();
            _VideoRepository = A.Fake<IVideoRepository>();
            _VideoCommentsLikeRepository = A.Fake<IVideoCommentsLikeRepository>();
            _VideoTagRepository = A.Fake<IVideoTagRepository>();
        }

        [Fact]
        public async Task VideoCommentController_GetVideoComment_ReturnOk()
        {
            int videoCommentId = 1;
            VideoCommentDto videoCommentDto = A.Fake<VideoCommentDto>();
            VideoComment videoComment = A.Fake<VideoComment>();

            A.CallTo(() => _VideoCommentRepository.GetVideoCommentAsync(videoCommentId))
                .Returns(videoComment);
            VideoCommentController controller = new VideoCommentController(_VideoCommentRepository,
                _Mapper, _UserRepository,
                _AuthService, _VideoRepository,
                _VideoCommentsLikeRepository, _VideoTagRepository);

            IActionResult result = await controller.GetVideoComment(videoCommentId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
