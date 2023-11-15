using AutoMapper;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkHubAPI.Controllers;
using TalkHubAPI.Dtos.ForumDtos;
using TalkHubAPI.Interfaces.ForumInterfaces;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Controllers.ForumControllers;
using FluentAssertions;

namespace TalkHubAPI.Tests.Controller.ForumControllerTests
{
    public class ForumThreadControllerTests
    {
        private readonly IForumThreadRepository _ForumThreadRepository;
        private readonly IMapper _Mapper;
        private readonly IMemoryCache _MemoryCache;
        public ForumThreadControllerTests()
        {
            _ForumThreadRepository = A.Fake<IForumThreadRepository>();
            _Mapper = A.Fake<IMapper>();
            _MemoryCache = A.Fake<MemoryCache>();
        }

        [Fact]
        public async Task ForumThreadController_GetThread_ReturnOk()
        {
            int threadId = 1;
            ForumThread thread = A.Fake<ForumThread>();

            A.CallTo(() => _ForumThreadRepository.GetForumThreadAsync(threadId)).Returns(thread);
            ForumThreadController controller = new ForumThreadController(_ForumThreadRepository, 
                _Mapper, _MemoryCache);

            IActionResult result = await controller.GetThread(threadId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
