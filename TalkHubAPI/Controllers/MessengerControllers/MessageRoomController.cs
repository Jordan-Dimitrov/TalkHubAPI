using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TalkHubAPI.Dto.ForumDtos;
using TalkHubAPI.Dto.MessengerDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.MessengerInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.MessengerModels;
using TalkHubAPI.Repository;

namespace TalkHubAPI.Controllers.MessengerControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageRoomController : Controller
    {
        private readonly IMessageRoomRepository _MessageRoomRepository;
        private readonly IMapper _Mapper;
        private readonly IAuthService _AuthService;
        private readonly IUserRepository _UserRepository;
        private readonly IUserMessageRoomRepository _UserMessageRoomRepository;

        public MessageRoomController(IMessageRoomRepository messageRoomRepository,
            IMapper mapper,
            IAuthService authService,
            IUserRepository userRepository,
            IUserMessageRoomRepository userMessageRoomRepository)
        {
            _MessageRoomRepository = messageRoomRepository;
            _Mapper = mapper;
            _AuthService = authService;
            _UserRepository = userRepository;
            _UserMessageRoomRepository = userMessageRoomRepository;
        }

        [HttpGet, Authorize(Roles = "Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MessageRoomDto>))]
        public IActionResult GetRooms()
        {
            ICollection<MessageRoomDto> rooms = _Mapper.Map<List<MessageRoomDto>>(_MessageRoomRepository.GetMessageRooms());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rooms);
        }

        [HttpGet("userRooms"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<MessageRoomDto>))]
        public IActionResult GetUserRooms()
        {

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

            ICollection<MessageRoomDto> rooms = _Mapper.Map<List<MessageRoomDto>>(_MessageRoomRepository
                .GetMessageRoomsForUser(user.Id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(rooms);
        }

        [HttpGet("{roomId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(MessageRoomDto))]
        [ProducesResponseType(400)]
        public IActionResult GetRoom(int roomId)
        {
            if (!_MessageRoomRepository.MessageRoomExists(roomId))
            {
                return NotFound();
            }

            MessageRoomDto room = _Mapper.Map<MessageRoomDto>(_MessageRoomRepository.GetMessageRoom(roomId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(room);
        }

        [HttpPost("createRoom"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateRoom([FromBody] MessageRoomDto roomCreate)
        {
            if (roomCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (_MessageRoomRepository.MessageRoomExists(roomCreate.RoomName))
            {
                ModelState.AddModelError("", "Room with such name already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MessageRoom room = _Mapper.Map<MessageRoom>(roomCreate);

            if (!_MessageRoomRepository.AddMessageRoom(room))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPost("joinRoom/{roomId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult JoinRoom(int roomId)
        {
            if (!_MessageRoomRepository.MessageRoomExists(roomId))
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
            MessageRoom room = _Mapper.Map<MessageRoom>(_MessageRoomRepository.GetMessageRoom(roomId));

            if (_UserMessageRoomRepository.UserMessageRoomExistsForRoomAndUser(room.Id, user.Id))
            {
                ModelState.AddModelError("", "User has already joined this room");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserMessageRoom userMessageRoom = new UserMessageRoom()
            {
                User = user,
                Room = room
            };

            if (!_UserMessageRoomRepository.AddUserMessageRoom(userMessageRoom))
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
        public IActionResult UpdateRoom(int roomId, [FromBody] MessageRoomDto updateRoom)
        {
            if (updateRoom == null || roomId != updateRoom.Id)
            {
                return BadRequest(ModelState);
            }

            if (!_MessageRoomRepository.MessageRoomExists(roomId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            MessageRoom roomToUpdate = _Mapper.Map<MessageRoom>(updateRoom);

            if (!_MessageRoomRepository.UpdateMessageRoom(roomToUpdate))
            {
                ModelState.AddModelError("", "Something went wrong updating the room");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{roomId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteRoom(int roomId)
        {
            if (!_MessageRoomRepository.MessageRoomExists(roomId))
            {
                return NotFound();
            }

            MessageRoom roomToDelete = _MessageRoomRepository.GetMessageRoom(roomId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_MessageRoomRepository.RemoveMessageRoom(roomToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting the room");
            }

            return NoContent();
        }

    }
}
