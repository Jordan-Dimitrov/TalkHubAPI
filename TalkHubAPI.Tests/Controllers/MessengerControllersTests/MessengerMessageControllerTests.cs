using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TalkHubAPI.Controllers.MessengerControllers;
using TalkHubAPI.Dtos.MessengerDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Tests.Controller.MessengerControllersTests
{
    public class MessengerMessageControllerTests
    {
        private readonly IMessageRoomRepository _MessageRoomRepository;
        private readonly IMessengerMessageRepository _MessengerMessageRepository;
        private readonly IMapper _Mapper;
        private readonly IAuthService _AuthService;
        private readonly IMemoryCache _MemoryCache;
        private readonly IUserRepository _UserRepository;
        private readonly IUserMessageRoomRepository _UserMessageRoomRepository;
        public MessengerMessageControllerTests()
        {
            _MessageRoomRepository = A.Fake<IMessageRoomRepository>();
            _UserMessageRoomRepository = A.Fake<IUserMessageRoomRepository>();
            _UserRepository = A.Fake<IUserRepository>();
            _MessengerMessageRepository = A.Fake<IMessengerMessageRepository>();
            _Mapper = A.Fake<IMapper>();
            _AuthService = A.Fake<IAuthService>();
            _MemoryCache = A.Fake<MemoryCache>();
        }

        [Fact]
        public async Task MessengerMessageController_GetMessagesFromRoom_ReturnOk()
        {
            int roomId = 1;
            User user = A.Fake<User>();
            user.Username = "user";
            MessageRoom room = A.Fake<MessageRoom>();
            ICollection<MessengerMessageDto> messageDto = A.Fake<ICollection<MessengerMessageDto>>();
            List<MessengerMessageDto> messagesList = A.Fake<List<MessengerMessageDto>>();
            List<MessengerMessage> messages = A.Fake<List<MessengerMessage>>();

            HttpContext httpContext = A.Fake<HttpContext>();
            HttpRequest fakeRequest = A.Fake<HttpRequest>();

            ControllerContext controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            A.CallTo(() => _MessageRoomRepository.GetMessageRoomAsync(roomId)).Returns(room);
            A.CallTo(() => fakeRequest.Cookies["jwtToken"]).Returns("fakeToken");
            A.CallTo(() => httpContext.Request).Returns(fakeRequest);
            A.CallTo(() => _AuthService.GetUsernameFromJwtToken("fakeToken")).Returns(user.Username);
            A.CallTo(() => _UserMessageRoomRepository
            .UserMessageRoomExistsForRoomAndUserAsync(roomId, user.Id)).Returns(true);
            A.CallTo(() => _MessengerMessageRepository.GetMessengerMessagesByRoomIdAsync(roomId))
                .Returns(messages);

            MessageRoomController controller = new MessageRoomController(_MessageRoomRepository, _Mapper,
                _AuthService, _UserRepository,
                _UserMessageRoomRepository, _MemoryCache)
            {
                ControllerContext = controllerContext
            };

            IActionResult result = await controller.GetRoom(roomId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
