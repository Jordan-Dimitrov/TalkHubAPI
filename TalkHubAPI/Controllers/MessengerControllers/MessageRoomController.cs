using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Threading;
using TalkHubAPI.Dtos.ForumDtos;
using TalkHubAPI.Dtos.MessengerDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.MessengerModels;

namespace TalkHubAPI.Controllers.MessengerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageRoomController : Controller
    {
        private readonly IMessageRoomRepository _MessageRoomRepository;
        private readonly IMapper _Mapper;
        private readonly string _MessageRoomsCacheKey;
        private readonly IMemoryCache _MemoryCache;
        private readonly IAuthService _AuthService;
        private readonly IUserRepository _UserRepository;
        private readonly IUserMessageRoomRepository _UserMessageRoomRepository;

        public MessageRoomController(IMessageRoomRepository messageRoomRepository,
            IMapper mapper,
            IAuthService authService,
            IUserRepository userRepository,
            IUserMessageRoomRepository userMessageRoomRepository,
            IMemoryCache memoryCache)
        {
            _MessageRoomRepository = messageRoomRepository;
            _Mapper = mapper;
            _AuthService = authService;
            _UserRepository = userRepository;
            _UserMessageRoomRepository = userMessageRoomRepository;
            _MemoryCache = memoryCache;
            _MessageRoomsCacheKey = "messageRooms";
        }

        [HttpGet, Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MessageRoomDto>))]
        public async Task<IActionResult> GetRooms()
        {
            ICollection<MessageRoomDto>? rooms = _MemoryCache.Get<List<MessageRoomDto>>(_MessageRoomsCacheKey);

            if (rooms is null)
            {
                rooms = _Mapper.Map<List<MessageRoomDto>>(await _MessageRoomRepository
                    .GetMessageRoomsAsync());

                _MemoryCache.Set(_MessageRoomsCacheKey, rooms, TimeSpan.FromMinutes(1));
            }

            return Ok(rooms);
        }

        [HttpGet("user-rooms"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MessageRoomDto>))]
        public async Task<IActionResult> GetUserRooms()
        {

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

            ICollection<MessageRoomDto> rooms = _Mapper.Map<List<MessageRoomDto>>(await _MessageRoomRepository
                .GetMessageRoomsForUserAsync(user.Id));

            return Ok(rooms);
        }

        [HttpGet("{roomId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(MessageRoomDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetRoom(int roomId)
        {
            MessageRoomDto room = _Mapper.Map<MessageRoomDto>(await _MessageRoomRepository
                .GetMessageRoomAsync(roomId));

            if (room is null)
            {
                return NotFound();
            }

            return Ok(room);
        }

        [HttpPost, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateRoom([FromBody] MessageRoomDto roomCreate)
        {
            if (roomCreate is null)
            {
                return BadRequest(ModelState);
            }

            if (await _MessageRoomRepository.MessageRoomExistsAsync(roomCreate.RoomName))
            {
                ModelState.AddModelError("", "Room with such name already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MessageRoom room = _Mapper.Map<MessageRoom>(roomCreate);

            if (!await _MessageRoomRepository.AddMessageRoomAsync(room))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_MessageRoomsCacheKey);

            return Ok("Successfully created");
        }

        [HttpPost("joinRoom"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> JoinRoom([FromBody]int roomId)
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

            if (await _UserMessageRoomRepository.UserMessageRoomExistsForRoomAndUserAsync(room.Id, user.Id))
            {
                ModelState.AddModelError("", "User has already joined this room");
                return StatusCode(422, ModelState);
            }

            UserMessageRoom userMessageRoom = new UserMessageRoom();
            userMessageRoom.User = user;
            userMessageRoom.Room = room;

            if (!await _UserMessageRoomRepository.AddUserMessageRoomAsync(userMessageRoom))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPut("{roomId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRoom(int roomId, [FromBody] MessageRoomDto updateRoom)
        {
            if (updateRoom is null || roomId != updateRoom.Id)
            {
                return BadRequest(ModelState);
            }

            if (!await _MessageRoomRepository.MessageRoomExistsAsync(roomId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            MessageRoom roomToUpdate = _Mapper.Map<MessageRoom>(updateRoom);

            if (!await _MessageRoomRepository.UpdateMessageRoomAsync(roomToUpdate))
            {
                ModelState.AddModelError("", "Something went wrong updating the room");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_MessageRoomsCacheKey);

            return NoContent();
        }

        [HttpDelete("{roomId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRoom(int roomId)
        {
            MessageRoom? roomToDelete = await _MessageRoomRepository.GetMessageRoomAsync(roomId);

            if (roomToDelete is null)
            {
                return NotFound();
            }

            if (!await _UserMessageRoomRepository.RemoveUserMessageRoomForRoomId(roomId) 
                || !await _MessageRoomRepository.RemoveMessageRoomAsync(roomToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the room");
            }

            _MemoryCache.Remove(_MessageRoomsCacheKey);

            return NoContent();
        }

    }
}
