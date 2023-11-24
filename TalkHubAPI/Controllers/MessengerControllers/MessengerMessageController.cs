using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Data;
using TalkHubAPI.Dtos.MessengerDtos;
using TalkHubAPI.Helper;
using TalkHubAPI.Hubs;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Controllers.MessengerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessengerMessageController : Controller
    {
        private readonly IMessengerMessageRepository _MessengerMessageRepository;
        private readonly IFileProcessingService _FileProcessingService;
        private readonly IMapper _Mapper;
        private readonly IMessageRoomRepository _MessageRoomRepository;
        private readonly IUserMessageRoomRepository _UserMessageRoomRepository;
        private readonly IAuthService _AuthService;
        private readonly IUserRepository _UserRepository;
        private readonly IHubContext<ChatHub> _HubContext;

        public MessengerMessageController(IMessengerMessageRepository messengerMessageRepository,
            IMapper mapper,
            IFileProcessingService fileProcessingService,
            IMessageRoomRepository messageRoomRepository,
            IUserMessageRoomRepository userMessageRoomRepository,
            IAuthService authService,
            IUserRepository userRepository,
            IHubContext<ChatHub> hubContext)
        {
            _MessengerMessageRepository = messengerMessageRepository;
            _Mapper = mapper;
            _FileProcessingService = fileProcessingService;
            _MessageRoomRepository = messageRoomRepository;
            _UserMessageRoomRepository = userMessageRoomRepository;
            _AuthService = authService;
            _UserRepository = userRepository;
            _HubContext = hubContext;
        }

        [HttpGet("{roomId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MessengerMessageDto>))]
        public async Task<IActionResult> GetMessagesFromRoom(int roomId)
        {
            MessageRoom? room = await _MessageRoomRepository.GetMessageRoomAsync(roomId);

            if (room is null)
            {
                return NotFound();
            }

            string? jwtToken = Request.Cookies["jwtToken"];
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            User? user = await _UserRepository.GetUserByNameAsync(username);

            if (user is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!await _UserMessageRoomRepository
                .UserMessageRoomExistsForRoomAndUserAsync(room.Id, user.Id) 
                && user.PermissionType != UserRole.Admin)
            {
                ModelState.AddModelError("", "User has not joined this room");
                return StatusCode(422, ModelState);
            }

            ICollection<MessengerMessageDto> messages = _Mapper.Map<List<MessengerMessageDto>>(await _MessengerMessageRepository
                .GetMessengerMessagesByRoomIdAsync(roomId));

            return Ok(messages);
        }

        [HttpGet("last-messages/{roomId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(MessageRoomDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetLastTenMessengerMessages(int roomId)
        {
            MessageRoom? room = await _MessageRoomRepository.GetMessageRoomAsync(roomId);

            if (room is null)
            {
                return NotFound();
            }

            string? jwtToken = Request.Cookies["jwtToken"];
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            User? user = await _UserRepository.GetUserByNameAsync(username);

            if (user is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!!await _UserMessageRoomRepository
                .UserMessageRoomExistsForRoomAndUserAsync(room.Id, user.Id)
                && user.PermissionType != UserRole.Admin)
            {
                ModelState.AddModelError("", "User has not joined this room");
                return StatusCode(422, ModelState);
            }

            if (!await _MessageRoomRepository.MessageRoomExistsAsync(roomId))
            {
                return NotFound();
            }

            MessengerMessage? messengerMessage = await _MessengerMessageRepository.GetLastMessageAsync();

            ICollection<MessengerMessageDto> messages = _Mapper.Map<List<MessengerMessageDto>>(await _MessengerMessageRepository
                .GetLastTenMessengerMessagesFromLastMessageIdAsync(messengerMessage.Id, roomId));

            return Ok(messages);
        }

        [HttpPost("message"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SendMessengerMessage([FromForm] SendMessengerMessageDto messageDto)
        {
            if (messageDto is null)
            {
                return BadRequest(ModelState);
            }

            if (!await _MessageRoomRepository.MessageRoomExistsAsync(messageDto.RoomId))
            {
                return BadRequest("MessageRoom does not exist");
            }

            string? jwtToken = Request.Cookies["jwtToken"];
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            User? user = await _UserRepository.GetUserByNameAsync(username);

            if (user is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!await _UserMessageRoomRepository
                .UserMessageRoomExistsForRoomAndUserAsync(messageDto.RoomId, user.Id)
                && user.PermissionType != UserRole.Admin)
            {
                ModelState.AddModelError("", "User has not joined this room");
                return StatusCode(422, ModelState);
            }

            if(messageDto.MessageContent is null)
            {
                return BadRequest("Invalid message");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MessageRoom room = _Mapper.Map<MessageRoom>(await _MessageRoomRepository
                .GetMessageRoomAsync(messageDto.RoomId));

            MessengerMessage message = _Mapper.Map<MessengerMessage>(messageDto);
            message.User = user;
            message.Room = room;
            message.DateCreated = DateTime.Now;

            if (!await _MessengerMessageRepository.AddMessengerMessageAsync(message))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            await _HubContext.Clients.Group(message.Room.RoomName)
                .SendAsync("RecieveMessage", message.MessageContent);

            return Ok();
        }

        [HttpPost("message-with-file"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> SendMessengerMessageWithFile(IFormFile file, [FromForm] SendMessengerMessageDto messageDto)
        {

            if (messageDto is null || !_FileProcessingService.ImageMimeTypeValid(file))
            {
                return BadRequest(ModelState);
            }

            if (!await _MessageRoomRepository.MessageRoomExistsAsync(messageDto.RoomId))
            {
                return BadRequest("MessageRoom does not exist");
            }

            string? jwtToken = Request.Cookies["jwtToken"];
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            User? user = await _UserRepository.GetUserByNameAsync(username);

            if (user is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!await _UserMessageRoomRepository
                .UserMessageRoomExistsForRoomAndUserAsync(messageDto.RoomId, user.Id)
                && user.PermissionType != UserRole.Admin)
            {
                ModelState.AddModelError("", "User has not joined this room");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MessageRoom room = _Mapper.Map<MessageRoom>(await _MessageRoomRepository
                .GetMessageRoomAsync(messageDto.RoomId));

            MessengerMessage message = _Mapper.Map<MessengerMessage>(messageDto);
            message.User = user;
            message.Room = room;
            message.DateCreated = DateTime.Now;

            string response = await _FileProcessingService.UploadImageAsync(file);

            if (response == "File already exists")
            {
                return BadRequest(response);
            }

            message.FileName = response;

            if (!await _MessengerMessageRepository.AddMessengerMessageAsync(message))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            using MemoryStream memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);

            ImageMessage image = new ImageMessage();
            image.ImageHeaders = "data: " + file.ContentType + ";base64,";
            image.ImageBinary = memoryStream.ToArray();

            await _HubContext.Clients.Group(message.Room.RoomName)
                .SendAsync("ReceiveFile", image);

            return Ok();
        }

        [HttpPut("hide/{messageId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> HideMessage(int messageId)
        {
            MessengerMessage? messageToHide = await _MessengerMessageRepository
                .GetMessengerMessageAsync(messageId);

            if (messageToHide is null)
            {
                return NotFound();
            }

            if (messageToHide.FileName is not null)
            {
                if (!await _FileProcessingService.RemoveMediaAsync(messageToHide.FileName))
                {
                    return BadRequest("Unexpected error");
                }
                messageToHide.FileName = "hidden.png";
            }

            messageToHide.MessageContent = "Message was hidden";

            if (!await _MessengerMessageRepository.UpdateMessengerMessageAsync(messageToHide))
            {
                ModelState.AddModelError("", "Something went wrong updating message");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
