using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TalkHubAPI.Controllers.MessengerControllers;
using TalkHubAPI.Dtos.MessengerDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Tests.Controller.MessengerControllersTests
{
    public class MessageRoomControllerTests
    {
        private readonly IMessageRoomRepository _MessageRoomRepository;
        private readonly IMapper _Mapper;
        private readonly IAuthService _AuthService;
        private readonly IMemoryCache _MemoryCache;
        private readonly IUserRepository _UserRepository;
        private readonly IUserMessageRoomRepository _UserMessageRoomRepository;
        public MessageRoomControllerTests()
        {
            _MessageRoomRepository = A.Fake<IMessageRoomRepository>();
            _UserMessageRoomRepository = A.Fake<IUserMessageRoomRepository>();
            _UserRepository = A.Fake<IUserRepository>();
            _Mapper = A.Fake<IMapper>();
            _AuthService = A.Fake<IAuthService>();
            _MemoryCache = A.Fake<MemoryCache>();
        }

        [Fact]
        public async Task MessageRoomController_GetRoom_ReturnOk()
        {
            int roomId = 1;
            MessageRoomDto messageDto = A.Fake<MessageRoomDto>();
            MessageRoom message = A.Fake<MessageRoom>();
            A.CallTo(() => _MessageRoomRepository.GetMessageRoomAsync(roomId)).Returns(message);

            MessageRoomController controller = new MessageRoomController(_MessageRoomRepository, _Mapper,
                _AuthService, _UserRepository,
                _UserMessageRoomRepository, _MemoryCache);

            IActionResult result = await controller.GetRoom(roomId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
