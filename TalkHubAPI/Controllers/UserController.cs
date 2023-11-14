using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics.Metrics;
using System.IdentityModel.Tokens.Jwt;
using TalkHubAPI.Dto.UserDtos;
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
        private readonly string _UserCacheKey;
        private readonly IMemoryCache _MemoryCache;
        private readonly IAuthService _AuthService;
        public UserController(IUserRepository userRepository,
            IMapper mapper,
            IAuthService authService,
            IMemoryCache memoryCache)
        {
            _UserRepository = userRepository;
            _Mapper = mapper;
            _AuthService = authService;
            _MemoryCache = memoryCache;
            _UserCacheKey = "users";
        }

        [HttpGet, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        public async Task<IActionResult> GetUsers()
        {
            ICollection<UserDto>? users = _MemoryCache.Get<List<UserDto>>(_UserCacheKey);

            if(users is null)
            {
                users = _Mapper.Map<List<UserDto>>(await _UserRepository.GetUsersAsync());

                _MemoryCache.Set(_UserCacheKey, users, TimeSpan.FromMinutes(1));
            }
 
            return Ok(users);
        }

        [HttpGet("{userId}"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(UserDto))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetUser(int userId)
        {
            UserDto user = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(userId));

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] CreateUserDto request)
        {
            if (request is null)
            {
                return BadRequest(ModelState);
            }

            if (await _UserRepository.UsernameExistsAsync(request.Username))
            {
                return BadRequest("User already exists!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserPassword pass = _AuthService.CreatePasswordHash(request.Password);

            User user = _Mapper.Map<User>(request);
            user.PasswordHash = pass.PasswordHash;
            user.PasswordSalt = pass.PasswordSalt;
            user.PermissionType = 0;

            if (!await _UserRepository.CreateUserAsync(user))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_UserCacheKey);

            return Ok("Successfully created");
        }

        [HttpGet("role"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetUserRole()
        {
            string jwtToken = Request.Headers["Authorization"].ToString().Replace("bearer ", "");
            string username = _AuthService.GetUsernameFromJwtToken(jwtToken);
            string role = _AuthService.GetRoleFromJwtToken(jwtToken);

            if (username is null)
            {
                return BadRequest(ModelState);
            }

            if (!await _UserRepository.UsernameExistsAsync(username))
            {
                return BadRequest("User with such name does not exist!");
            }

            return Ok(role);
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login(CreateUserDto request)
        {
            if (request is null)
            {
                return BadRequest(ModelState);
            }

            User? user = await _UserRepository.GetUserByNameAsync(request.Username);

            if (user is null)
            {
                return BadRequest("User with such name does not exist!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_AuthService.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            string token = _AuthService.GenerateJwtToken(user);

            RefreshToken refreshToken = _AuthService.GenerateRefreshToken();

            if (!await _UserRepository.UpdateRefreshTokenToUserAsync(user, refreshToken))
            {
                ModelState.AddModelError("", "Something went wrong while updating the refresh token");
                return StatusCode(500, ModelState);
            } 

            _AuthService.SetRefreshToken(refreshToken);

            return Ok(token);
        }

        [HttpPost("refresh-token")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetRefreshToken()
        {

            string refreshToken = Request.Cookies["refreshToken"];

            User? user = await _UserRepository.GetUserByRefreshTokenAsync(refreshToken);

            if (user is null)
            {
                return Unauthorized("Invalid Refresh Token.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (user.RefreshToken.TokenExpires < DateTime.UtcNow)
            {
                return Unauthorized("Token expired.");
            }

            string token = _AuthService.GenerateJwtToken(user);

            RefreshToken newRefreshToken = _AuthService.GenerateRefreshToken();

            if (!await _UserRepository.UpdateRefreshTokenToUserAsync(user, newRefreshToken))
            {
                ModelState.AddModelError("", "Something went wrong while updating the refresh token");
                return StatusCode(500, ModelState);
            }

            _AuthService.SetRefreshToken(newRefreshToken);

            return Ok(token);
        }
    }
}
