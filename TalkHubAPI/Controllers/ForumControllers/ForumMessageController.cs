using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using TalkHubAPI.Dto.ForumDtos;
using TalkHubAPI.Dto.PhotosDtos;
using TalkHubAPI.Dto.UserDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.ForumInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Repository;

namespace TalkHubAPI.Controllers.ForumControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumMessageController : Controller
    {
        private readonly IForumMessageRepository _ForumMessageRepository;
        private readonly IMapper _Mapper;
        private readonly IFileProcessingService _FileProcessingService;
        private readonly IUserRepository _UserRepository;
        private readonly IAuthService _AuthService;
        private readonly IForumThreadRepository _ForumThreadRepository;
        private readonly IUserUpvoteRepository _UserUpvoteRepository;
        public ForumMessageController(IForumMessageRepository forumMessageRepository,
            IMapper mapper,
            IFileProcessingService fileProcessingService,
            IUserRepository userRepository,
            IAuthService authService,
            IForumThreadRepository forumThreadRepository,
            IUserUpvoteRepository userUpvoteRepository)
        {
            _ForumMessageRepository = forumMessageRepository;
            _Mapper = mapper;
            _FileProcessingService = fileProcessingService;
            _UserRepository = userRepository;
            _AuthService = authService;
            _ForumThreadRepository = forumThreadRepository;
            _UserUpvoteRepository = userUpvoteRepository;
        }

        [HttpPost("addMedia"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public IActionResult CreateMessage(CreateForumMessageDto messageDto)
        {
            if (messageDto == null)
            {
                return BadRequest(ModelState);
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

            if (!_ForumThreadRepository.ForumThreadExists(messageDto.ForumThreadId))
            {
                return BadRequest("This thread does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ForumMessage message = _Mapper.Map<ForumMessage>(messageDto);
            message.ForumThread = _Mapper.Map<ForumThread>(_ForumThreadRepository.GetForumThread(message.ForumThreadId));

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
            message.DateCreated = DateTime.Now;
            message.User = user;
            message.UpvoteCount = 0;

            if (!_ForumMessageRepository.AddForumMessage(message))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpPost("addMediaWithFile"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public IActionResult CreateMessageWithFile(IFormFile file, [FromForm] CreateForumMessageDto messageDto)
        {
            if (file == null || file.Length == 0 || messageDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_FileProcessingService.GetContentType(file.FileName) == "video/mp4")
            {
                return BadRequest(ModelState);
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

            if (!_ForumThreadRepository.ForumThreadExists(messageDto.ForumThreadId))
            {
                return BadRequest("This thread does not exist");
            }

            string response = _FileProcessingService.UploadMedia(file);

            if (response == "Empty" || response == "Invalid file format" || response == "File already exists")
            {
                return BadRequest(response);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ForumMessage message = _Mapper.Map<ForumMessage>(messageDto);
            message.ForumThread = _Mapper.Map<ForumThread>(_ForumThreadRepository.GetForumThread(message.ForumThreadId));

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(username));
            message.FileName = file.FileName;
            message.DateCreated = DateTime.Now;
            message.User = user;
            message.UpvoteCount = 0;

            if (!_ForumMessageRepository.AddForumMessage(message))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

        [HttpGet("forumMessage/{threadId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<PhotoDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetAllMessagesByForumThread(int threadId)
        {
            if (!_ForumThreadRepository.ForumThreadExists(threadId))
            {
                return BadRequest("This thread does not exist");
            }

            List<ForumMessage> messages = _ForumMessageRepository.GetForumMessagesByForumThreadId(threadId).ToList();
            List<ForumMessageDto> messageDto = _Mapper.Map<List<ForumMessageDto>>(_ForumMessageRepository.GetForumMessagesByForumThreadId(threadId).ToList());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            for (int i = 0; i < messages.Count; i++)
            {
                messageDto[i].ForumThread = _Mapper.Map<ForumThreadDto>(_ForumThreadRepository.GetForumThread(messages[i].ForumThreadId));
                messageDto[i].User = _Mapper.Map<UserDto>(_UserRepository.GetUser(messages[i].UserId));
            }

            return Ok(messageDto);
        }

        [HttpGet("{fileName}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(void), 404)]
        public IActionResult GetMedia(string fileName)
        {
            if (_FileProcessingService.GetContentType(fileName) == "video/mp4")
            {
                return BadRequest(ModelState);
            }

            FileContentResult file = _FileProcessingService.GetMedia(fileName);

            if (file == null)
            {
                return NotFound();
            }

            return file;
        }
        [HttpPut("hide/{forumMessageId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult HideMessage(int forumMessageId)
        {
            if (!_ForumMessageRepository.ForumMessageExists(forumMessageId))
            {
                return NotFound();
            }

            ForumMessage messageToHide = _ForumMessageRepository.GetForumMessage(forumMessageId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (messageToHide.FileName!=null)
            {
                if (!_FileProcessingService.RemoveMedia(messageToHide.FileName))
                {
                    return BadRequest("Unexpected error");
                }
                messageToHide.FileName = "hidden.png";
            }

            messageToHide.MessageContent = "Message was hidden";

            if (!_ForumMessageRepository.UpdateForumMessage(messageToHide))
            {
                ModelState.AddModelError("", "Something went wrong updating message");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpPut("upvoteForumMessage/{forumMessageId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public IActionResult UpvoteMessage([FromQuery] int upvoteValue, int forumMessageId)
        {
            if (upvoteValue != 1 && upvoteValue != -1)
            {
                return BadRequest(ModelState);
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

            if (!_ForumMessageRepository.ForumMessageExists(forumMessageId))
            {
                return BadRequest("This message does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ForumMessage message = _ForumMessageRepository.GetForumMessage(forumMessageId);

            UserUpvote upvote = new UserUpvote();
            upvote.Message = message;
            upvote.User = _UserRepository.GetUserByName(username);

            if (_UserUpvoteRepository.UserUpvoteExistsForMessageAndUser(message.Id, message.UserId))
            {
                upvote = _UserUpvoteRepository.GetUserUpvoteByMessageAndUser(message.Id, message.UserId);
                if (upvoteValue == upvote.Rating)
                {
                    upvote.Rating = 0;
                    message.UpvoteCount -= upvoteValue;
                }
                else
                {
                    int temp = upvote.Rating;
                    upvote.Rating = upvoteValue;
                    message.UpvoteCount += upvoteValue - temp;
                }
            }
            else
            {
                upvote.Rating = upvoteValue;
                message.UpvoteCount += upvoteValue;
                if (!_UserUpvoteRepository.AddUserUpvote(upvote))
                {
                    return BadRequest(ModelState);
                }
            }

            if (!_ForumMessageRepository.UpdateForumMessage(message) || !_UserUpvoteRepository.UpdateUserUpvote(upvote))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
