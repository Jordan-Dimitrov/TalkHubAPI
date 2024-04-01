using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TalkHubAPI.Controllers.ForumControllers;
using TalkHubAPI.Dtos.ForumDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.ForumInterfaces;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Models.ForumModels;

namespace TalkHubAPI.Tests.Controller.ForumControllerTests
{
    public class ForumMessageControllerTests
    {
        private readonly IForumMessageRepository _ForumMessageRepository;
        private readonly IUserRepository _UserRepository;
        private readonly IForumThreadRepository _ForumThreadRepository;
        private readonly IMapper _Mapper;
        private readonly IAuthService _AuthService;
        private readonly IMemoryCache _MemoryCache;
        private readonly IFileProcessingService _FileProcessingService;
        private readonly IUserUpvoteRepository _UserUpvoteRepository;
        public ForumMessageControllerTests()
        {
            _ForumMessageRepository = A.Fake<IForumMessageRepository>();
            _UserRepository = A.Fake<IUserRepository>();
            _ForumThreadRepository = A.Fake<IForumThreadRepository>();
            _FileProcessingService = A.Fake<IFileProcessingService>();
            _UserUpvoteRepository = A.Fake<IUserUpvoteRepository>();
            _Mapper = A.Fake<IMapper>();
            _AuthService = A.Fake<IAuthService>();
            _MemoryCache = A.Fake<MemoryCache>();
        }

        [Fact]
        public async Task ForumMessageController_GetForumMessage_ReturnOk()
        {
            int messageId = 1;
            ForumMessageDto messageDto = A.Fake<ForumMessageDto>();
            ForumMessage message = A.Fake<ForumMessage>();
            A.CallTo(() => _ForumMessageRepository.GetForumMessageAsync(messageId)).Returns(message);

            ForumMessageController controller = new ForumMessageController(_ForumMessageRepository,
                _Mapper, _FileProcessingService,
                _UserRepository, _AuthService,
                _ForumThreadRepository, _UserUpvoteRepository);

            IActionResult result = await controller.GetForumMessage(messageId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
