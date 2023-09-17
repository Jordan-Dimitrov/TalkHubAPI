using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TalkHubAPI.Dto.MessengerDtos;
using TalkHubAPI.Helper;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.MessengerModels;
using TalkHubAPI.Repository;
using TalkHubAPI.Repository.MessengerRepositories;

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

        public MessengerMessageController(IMessengerMessageRepository messengerMessageRepository,
            IMapper mapper,
            IFileProcessingService fileProcessingService,
            IMessageRoomRepository messageRoomRepository,
            IUserMessageRoomRepository userMessageRoomRepository,
            IAuthService authService,
            IUserRepository userRepository)
        {
            _MessengerMessageRepository = messengerMessageRepository;
            _Mapper = mapper;
            _FileProcessingService = fileProcessingService;
            _MessageRoomRepository = messageRoomRepository;
            _UserMessageRoomRepository = userMessageRoomRepository;
            _AuthService = authService;
            _UserRepository = userRepository;
        }

        [HttpGet("messages/{roomId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MessengerMessageDto>))]
        public async Task<IActionResult> GetMessagesFromRoom(int roomId)
        {
            if (!await _MessageRoomRepository.MessageRoomExistsAsync(roomId))
            {
                return NotFound();
            }

            string jwtToken = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null)
            {
                return BadRequest(ModelState);
            }

            if (!_UserRepository.UsernameExists(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
            MessageRoom room = _Mapper.Map<MessageRoom>(await _MessageRoomRepository.GetMessageRoomAsync(roomId));

            if (!await _UserMessageRoomRepository.UserMessageRoomExistsForRoomAndUserAsync(room.Id, user.Id))
            {
                ModelState.AddModelError("", "User has not joined this room");
                return StatusCode(422, ModelState);
            }

            ICollection<MessengerMessageDto> messages = _Mapper.Map<List<MessengerMessageDto>>(await _MessengerMessageRepository
                .GetMessengerMessagesByRoomIdAsync(roomId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(messages);
        }

        [HttpGet("lastTenMessagesById/{messageId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(MessageRoomDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetLastTenMessengerMessages([FromQuery] int roomId, int messageId)
        {
            if (!await _MessageRoomRepository.MessageRoomExistsAsync(roomId))
            {
                return NotFound();
            }

            string jwtToken = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null)
            {
                return BadRequest(ModelState);
            }

            if (!_UserRepository.UsernameExists(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
            MessageRoom room = _Mapper.Map<MessageRoom>(await _MessageRoomRepository.GetMessageRoomAsync(roomId));

            if (!await _UserMessageRoomRepository.UserMessageRoomExistsForRoomAndUserAsync(room.Id, user.Id))
            {
                ModelState.AddModelError("", "User has not joined this room");
                return StatusCode(422, ModelState);
            }

            if (!await _MessageRoomRepository.MessageRoomExistsAsync(roomId))
            {
                return NotFound();
            }

            ICollection<MessengerMessageDto> messages = _Mapper.Map<List<MessengerMessageDto>>(await _MessengerMessageRepository
                .GetLastTenMessengerMessagesFromLastMessageIdAsync(messageId, roomId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(messages);
        }

        [HttpPut("hide/{messageId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> HideMessage(int messageId)
        {
            if (!await _MessengerMessageRepository.MessengerMessageExistsAsync(messageId))
            {
                return NotFound();
            }

            MessengerMessage messageToHide = await _MessengerMessageRepository.GetMessengerMessageAsync(messageId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (messageToHide.FileName != null)
            {
                if (!_FileProcessingService.RemoveMedia(messageToHide.FileName))
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
