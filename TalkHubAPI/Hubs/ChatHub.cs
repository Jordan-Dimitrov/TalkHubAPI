using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using TalkHubAPI.Dtos.MessengerDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageRoomRepository _MessageRoomRepository;
        private readonly IMessengerMessageRepository _MessengerMessageRepository;
        private readonly IUserMessageRoomRepository _UserMessageRoomRepository;
        private readonly IMapper _Mapper;
        private readonly IAuthService _AuthService;
        private readonly IFileProcessingService _FileProcessingService;
        private readonly IUserRepository _UserRepository;
        public ChatHub(IMessageRoomRepository messageRoomRepository,
            IMessengerMessageRepository messengerMessageRepository,
            IUserMessageRoomRepository userMessageRoomRepository,
            IMapper mapper,
            IAuthService authService,
            IFileProcessingService fileProcessingService,
            IUserRepository userRepository)
        {
            _MessageRoomRepository = messageRoomRepository;
            _MessengerMessageRepository = messengerMessageRepository;
            _UserMessageRoomRepository = userMessageRoomRepository;
            _Mapper = mapper;
            _AuthService = authService;
            _FileProcessingService = fileProcessingService;
            _UserRepository = userRepository;
        }
        [Authorize(Roles = "User,Admin")]
        public async Task TestMe(string temp)
        {
            await Clients.All.SendAsync($"{this.Context.User.Identity.Name} : {temp}",CancellationToken.None);
        }

        [Authorize(Roles = "User,Admin")]
        public async Task JoinRoom(MessageRoomDto request)
        {
            if(request is null)
            {
                Context.Abort();
                return;
            }

            if (!await _MessageRoomRepository.MessageRoomExistsAsync(request.RoomName))
            {
                Context.Abort();
                return;
            }

            User user = _Mapper.Map<User>(await _UserRepository.GetUserByNameAsync(this.Context.User.Identity.Name));
            MessageRoom room = _Mapper.Map<MessageRoom>(await _MessageRoomRepository
                .GetMessageRoomByNameAsync(request.RoomName));

            if (!await _UserMessageRoomRepository.UserMessageRoomExistsForRoomAndUserAsync(room.Id, user.Id))
            {
                Context.Abort();
                return;
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, request.RoomName);

            MessengerMessage lastMessage = await _MessengerMessageRepository.GetLastMessageAsync();

            string messageJson = JsonConvert.SerializeObject(_Mapper.Map<List<MessengerMessageDto>>(await _MessengerMessageRepository.
                GetLastTenMessengerMessagesFromLastMessageIdAsync(lastMessage.Id, request.Id)));

            await Clients.Caller.SendAsync(messageJson);
        }

        [Authorize(Roles = "User,Admin")]
        public Task LeaveRoom(MessageRoomDto request)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, request.RoomName);
        }
    }
}
