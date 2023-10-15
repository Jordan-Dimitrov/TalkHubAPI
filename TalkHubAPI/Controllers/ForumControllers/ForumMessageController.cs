using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Data;
using System.Threading;
using TalkHubAPI.Dto.ForumDtos;
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
        private readonly string _ForumMessagesCacheKey;
        private readonly IMemoryCache _MemoryCache;
        private readonly IForumThreadRepository _ForumThreadRepository;
        private readonly IUserUpvoteRepository _UserUpvoteRepository;
        public ForumMessageController(IForumMessageRepository forumMessageRepository,
            IMapper mapper,
            IFileProcessingService fileProcessingService,
            IUserRepository userRepository,
            IAuthService authService,
            IForumThreadRepository forumThreadRepository,
            IUserUpvoteRepository userUpvoteRepository,
            IMemoryCache memoryCache)
        {
            _ForumMessageRepository = forumMessageRepository;
            _Mapper = mapper;
            _FileProcessingService = fileProcessingService;
            _UserRepository = userRepository;
            _AuthService = authService;
            _ForumThreadRepository = forumThreadRepository;
            _UserUpvoteRepository = userUpvoteRepository;
            _MemoryCache = memoryCache;
            _ForumMessagesCacheKey = "forumMessages";
        }

        [HttpPost("forum-message"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> CreateMessage(CreateForumMessageDto messageDto)
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

            if (!await _UserRepository.UsernameExistsAsync(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!await _ForumThreadRepository.ForumThreadExistsAsync(messageDto.ForumThreadId))
            {
                return BadRequest("This thread does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ForumMessage message = _Mapper.Map<ForumMessage>(messageDto);
            message.ForumThread = _Mapper.Map<ForumThread>(await _ForumThreadRepository.GetForumThreadAsync(message.ForumThreadId));

            User user = _Mapper.Map<User>(await _UserRepository.GetUserByNameAsync(username));
            message.DateCreated = DateTime.Now;
            message.User = user;
            message.UpvoteCount = 0;

            if (!await _ForumMessageRepository.AddForumMessageAsync(message))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            string cacheKey = _ForumMessagesCacheKey + $"_{message.ForumThreadId}";
            _MemoryCache.Remove(cacheKey);

            return Ok("Successfully created");
        }

        [HttpPost("forum-message-with-file"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> CreateMessageWithFile(IFormFile file, [FromForm] CreateForumMessageDto messageDto)
        {
            if (file == null || file.Length == 0 || messageDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_FileProcessingService.GetContentType(file.FileName) != "image/webp")
            {
                return BadRequest(ModelState);
            }

            string jwtToken = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);

            if (username == null)
            {
                return BadRequest(ModelState);
            }

            if (!await _UserRepository.UsernameExistsAsync(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!await _ForumThreadRepository.ForumThreadExistsAsync(messageDto.ForumThreadId))
            {
                return BadRequest("This thread does not exist");
            }

            string response = await _FileProcessingService.UploadImageAsync(file);

            if (response == "Empty" || response == "Invalid file format" || response == "File already exists")
            {
                return BadRequest(response);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ForumMessage message = _Mapper.Map<ForumMessage>(messageDto);
            message.ForumThread = _Mapper.Map<ForumThread>(await _ForumThreadRepository.GetForumThreadAsync(message.ForumThreadId));

            User user = _Mapper.Map<User>(await _UserRepository.GetUserByNameAsync(username));
            message.FileName = response;
            message.DateCreated = DateTime.Now;
            message.User = user;
            message.UpvoteCount = 0;

            if (!await _ForumMessageRepository.AddForumMessageAsync(message))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            string cacheKey = _ForumMessagesCacheKey + $"_{message.ForumThreadId}";
            _MemoryCache.Remove(cacheKey);

            return Ok("Successfully created");
        }

        [HttpGet("thread/{threadId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ForumMessageDto>))]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetAllMessagesByForumThread(int threadId)
        {
            if (!await _ForumThreadRepository.ForumThreadExistsAsync(threadId))
            {
                return BadRequest("This thread does not exist");
            }

            string cacheKey = _ForumMessagesCacheKey + $"_{threadId}";
            List<ForumMessageDto> messageDtos = _MemoryCache.Get<List<ForumMessageDto>>(cacheKey);

            if (messageDtos == null)
            {
                List<ForumMessage> messages = (await _ForumMessageRepository.GetForumMessagesByForumThreadIdAsync(threadId)).ToList();
                messageDtos = _Mapper.Map<List<ForumMessageDto>>(messages);

                for (int i = 0; i < messages.Count; i++)
                {
                    messageDtos[i].ForumThread = _Mapper.Map<ForumThreadDto>(await _ForumThreadRepository.GetForumThreadAsync(messages[i].ForumThreadId));
                    messageDtos[i].User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(messages[i].UserId));
                }

                _MemoryCache.Set(cacheKey, messageDtos, TimeSpan.FromMinutes(1));
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(messageDtos);
        }

        [HttpGet("file/{fileName}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(void), 404)]
        public async Task<IActionResult> GetMedia(string fileName)
        {
            if (_FileProcessingService.GetContentType(fileName) == "video/mp4")
            {
                return BadRequest(ModelState);
            }

            FileContentResult file = await _FileProcessingService.GetImageAsync(fileName);

            if (file == null)
            {
                return NotFound();
            }

            return file;
        }

        [HttpGet("{forumMessageId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(ForumMessageDto))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetForumMessage(int forumMessageId)
        {
            if (!await _ForumMessageRepository.ForumMessageExistsAsync(forumMessageId))
            {
                return NotFound();
            }

            ForumMessage message = await _ForumMessageRepository.GetForumMessageAsync(forumMessageId);
            ForumMessageDto messageDto = _Mapper.Map<ForumMessageDto>(await _ForumMessageRepository.GetForumMessageAsync(forumMessageId));
            messageDto.User = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(message.UserId));
            messageDto.ForumThread = _Mapper.Map<ForumThreadDto>(await _ForumThreadRepository.GetForumThreadAsync(message.ForumThreadId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(messageDto);
        }

        [HttpPut("hide/{forumMessageId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> HideMessage(int forumMessageId)
        {
            if (!await _ForumMessageRepository.ForumMessageExistsAsync(forumMessageId))
            {
                return NotFound();
            }

            string cacheKey = _ForumMessagesCacheKey + $"_{forumMessageId}";
            ForumMessage messageToHide = await _ForumMessageRepository.GetForumMessageAsync(forumMessageId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (messageToHide.FileName!=null)
            {
                if (!await _FileProcessingService.RemoveMediaAsync(messageToHide.FileName))
                {
                    return BadRequest("Unexpected error");
                }
                messageToHide.FileName = "hidden.png";
            }

            messageToHide.MessageContent = "Message was hidden";

            if (!await _ForumMessageRepository.UpdateForumMessageAsync(messageToHide))
            {
                ModelState.AddModelError("", "Something went wrong updating message");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(cacheKey);

            return NoContent();
        }

        [HttpPut("upvote/{forumMessageId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [Authorize]
        public async Task<IActionResult> UpvoteMessage([FromQuery] int upvoteValue, int forumMessageId)
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

            if (!await _UserRepository.UsernameExistsAsync(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!await _ForumMessageRepository.ForumMessageExistsAsync(forumMessageId))
            {
                return BadRequest("This message does not exist");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string cacheKey = _ForumMessagesCacheKey + $"_{forumMessageId}";
            ForumMessage message = await _ForumMessageRepository.GetForumMessageAsync(forumMessageId);
            User user = await _UserRepository.GetUserByNameAsync(username);

            if (!await _UserUpvoteRepository.UserUpvoteExistsForMessageAndUserAsync(message.Id, user.Id))
            {
                UserUpvote upvoteToAdd = new UserUpvote();
                upvoteToAdd.Rating = upvoteValue;
                upvoteToAdd.Message = message;
                upvoteToAdd.User = user;
                message.UpvoteCount += upvoteValue;

                if (!await _UserUpvoteRepository.AddUserUpvoteAsync(upvoteToAdd))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                return NoContent();
            }

            UserUpvote upvote = await _UserUpvoteRepository.GetUserUpvoteByMessageAndUserAsync(message.Id, user.Id);

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

            if (!await _UserUpvoteRepository.UpdateUserUpvoteAsync(upvote) || !await _ForumMessageRepository.UpdateForumMessageAsync(message))
            {
                ModelState.AddModelError("", "Something went wrong while updating");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(cacheKey);

            return NoContent();
        }
    }
}
