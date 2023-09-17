using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using TalkHubAPI.Dto.MessengerDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.MessengerModels;
using TalkHubAPI.Repository;

namespace TalkHubAPI.Hubs
{
    //Not tested, probably won't work
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
        public async Task<List<MessengerMessageDto>> JoinRoom(MessageRoomDto request)
        {

            var jwtToken = Context.GetHttpContext().Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null ||
                !_UserRepository.UsernameExists(username) ||
                !await _MessageRoomRepository.MessageRoomExistsAsync(request.RoomName))
            {
                Context.Abort();
            }

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
            MessageRoom room = _Mapper.Map<MessageRoom>(await _MessageRoomRepository
                .GetMessageRoomByNameAsync(request.RoomName));

            if (!await _UserMessageRoomRepository.UserMessageRoomExistsForRoomAndUserAsync(room.Id, user.Id))
            {
                Context.Abort();
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, request.RoomName);

            MessengerMessage lastMessage = await _MessengerMessageRepository.GetLastMessageAsync();

            return _Mapper.Map<List<MessengerMessageDto>>(await _MessengerMessageRepository.
                GetLastTenMessengerMessagesFromLastMessageIdAsync(lastMessage.Id, request.Id));
        }
        [Authorize(Roles = "User,Admin")]

        public Task LeaveRoom(MessageRoomDto request)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, request.RoomName);
        }
        [Authorize(Roles = "User,Admin")]

        public Task SendMessage(SendMessengerMessageDto request, MessageRoomDto request2)
        {
            var jwtToken = Context.GetHttpContext().Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null || !_UserRepository.UsernameExists(username))
            {
                Context.Abort();
            }

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
            MessageRoom room = _Mapper.Map<MessageRoom>(_MessageRoomRepository
                .GetMessageRoomByNameAsync(request2.RoomName));

            MessengerMessage message = _Mapper.Map<MessengerMessage>(request);
            message.User = user;
            message.Room = room;
            message.DateCreated = DateTime.Now;

            _MessengerMessageRepository.AddMessengerMessageAsync(message);

            return Clients.GroupExcept(message.Room.RoomName, new[] { Context.ConnectionId })
                .SendAsync("send_message", message.MessageContent);
        }
    }
}
