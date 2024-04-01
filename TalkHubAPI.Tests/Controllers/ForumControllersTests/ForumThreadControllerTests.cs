using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TalkHubAPI.Controllers.ForumControllers;
using TalkHubAPI.Interfaces.ForumInterfaces;
using TalkHubAPI.Models.ConfigurationModels;
using TalkHubAPI.Models.ForumModels;

namespace TalkHubAPI.Tests.Controller.ForumControllerTests
{
    public class ForumThreadControllerTests
    {
        private readonly IForumThreadRepository _ForumThreadRepository;
        private readonly IMapper _Mapper;
        private readonly IMemoryCache _MemoryCache;
        private readonly IOptions<MemoryCacheSettings> _MemoryCacheSettings;

        public ForumThreadControllerTests()
        {
            _ForumThreadRepository = A.Fake<IForumThreadRepository>();
            _Mapper = A.Fake<IMapper>();
            _MemoryCache = A.Fake<MemoryCache>();
            _MemoryCacheSettings = A.Fake<IOptions<MemoryCacheSettings>>();
        }

        [Fact]
        public async Task ForumThreadController_GetThread_ReturnOk()
        {
            int threadId = 1;
            ForumThread thread = A.Fake<ForumThread>();

            A.CallTo(() => _ForumThreadRepository.GetForumThreadAsync(threadId)).Returns(thread);
            ForumThreadController controller = new ForumThreadController(_ForumThreadRepository,
                _Mapper, _MemoryCache, _MemoryCacheSettings);

            IActionResult result = await controller.GetThread(threadId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
