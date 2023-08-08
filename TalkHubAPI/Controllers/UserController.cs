using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using TalkHubAPI.Dto;
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
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Register([FromBody]CreateUserDto request)
        {
            if (request==null)
            {
                return BadRequest(ModelState);
            }
            if (_UserRepository.UsernameExists(request.Username))
            {
                return BadRequest("User already exists!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _AuthService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = _Mapper.Map<User>(request);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PermissionType = 0;

            if (!_UserRepository.CreateUser(user))
            {
                return BadRequest(ModelState);
            }

            return Ok(request.Username);
        }
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult Login(CreateUserDto request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }

            if (!_UserRepository.UsernameExists(request.Username))
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(request.Username));

            if (!_AuthService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            string token = _AuthService.GenerateJwtToken(user);

            RefreshToken refreshToken = _AuthService.GenerateRefreshToken();
            if (!_UserRepository.UpdateRefreshTokenToUser(user, refreshToken))
            {
                return BadRequest(ModelState);
            }
            SetRefreshToken(refreshToken);
            return Ok(token);
        }
        [HttpPost("refresh-token")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetRefreshToken(UserDto request)
        {
            if (request == null)
            {
                return BadRequest(ModelState);
            }

            var refreshToken = Request.Cookies["refreshToken"];

            if (!_UserRepository.UsernameExists(request.Username))
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _Mapper.Map<User>(_UserRepository.GetUserByName(request.Username));

            if (!user.RefreshToken.Token.Equals(refreshToken))
            {
                return Unauthorized("Invalid Refresh Token.");
            }
            else if (user.RefreshToken.TokenExpires < DateTime.Now)
            {
                return Unauthorized("Token expired.");
            }

            string token = _AuthService.GenerateJwtToken(user);

            RefreshToken newRefreshToken = _AuthService.GenerateRefreshToken();
            if (!_UserRepository.UpdateRefreshTokenToUser(user, newRefreshToken))
            {
                return BadRequest(ModelState);
            }

            SetRefreshToken(newRefreshToken);
            return Ok(token);
        }
        [HttpGet("test"), Authorize(Roles = "Admin")]
        
        public IActionResult AuthTest()
        {
            return Ok(1);
        }
        private void SetRefreshToken(RefreshToken newRefreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.TokenExpires
            };
            Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }
    }
}
