using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using TalkHubAPI.Dto;
using TalkHubAPI.Helper;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;

namespace TalkHubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserRepository _UserRepository;
        private readonly IMapper _Mapper;
        private readonly IAuthService _AuthService;
        public UserController(IUserRepository userRepository, IMapper mapper, IAuthService authService)
        {
            _UserRepository = userRepository;
            _Mapper = mapper;
            _AuthService = authService;
        }
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        public IActionResult GetUsers()
        {
            var users = _Mapper.Map<List<UserDto>>(_UserRepository.GetUsers());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(users);
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(400)]
        public IActionResult GetUser(int userId)
        {
            if (!_UserRepository.UserExists(userId))
            {
                return NotFound();
            }

            var user = _Mapper.Map<UserDto>(_UserRepository.GetUser(userId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(user);
        }

        [HttpPost("register")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult Register(UserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_UserRepository.UsernameExists(request.Username))
            {
                return BadRequest("User already exists!");
            }
            _UserRepository.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User
            {
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };

            _UserRepository.CreateUser(user);
            return Ok(user);
        }
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Login(UserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_UserRepository.UsernameExists(request.Username))
            {
                return BadRequest("User with such name does not exist!");
            }

            User user = _UserRepository.GetUserByName(request.Username);
            if (!_UserRepository.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            string token = _AuthService.CreateJwtToken(user);

            return Ok(token);
        }
    }
}
