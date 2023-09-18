    using AutoMapper;
    using Azure.Core;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Diagnostics.Metrics;
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
            private readonly IAuthService _AuthService;
            public UserController(IUserRepository userRepository, IMapper mapper, IAuthService authService)
            {
                _UserRepository = userRepository;
                _Mapper = mapper;
                _AuthService = authService;
            }

            [HttpGet, Authorize(Roles = "User,Admin")]
            [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
            public async Task<IActionResult> GetUsers()
            {
                ICollection<UserDto> users = _Mapper.Map<List<UserDto>>(await _UserRepository.GetUsersAsync());

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(users);
            }

            [HttpGet("{userId}"), Authorize(Roles = "User,Admin")]
            [ProducesResponseType(200, Type = typeof(UserDto))]
            [ProducesResponseType(400)]
            public async Task<IActionResult> GetUser(int userId)
            {
                if (!await _UserRepository.UserExistsAsync(userId))
                {
                    return NotFound();
                }

                UserDto user = _Mapper.Map<UserDto>(await _UserRepository.GetUserAsync(userId));

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                return Ok(user);
            }

            [HttpPost("register")]
            [ProducesResponseType(201)]
            [ProducesResponseType(400)]
            public async Task<IActionResult> Register([FromBody]CreateUserDto request)
            {
                if (request==null)
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

                _AuthService.CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                User user = _Mapper.Map<User>(request);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.PermissionType = 0;

                if (!await _UserRepository.CreateUserAsync(user))
                {
                    ModelState.AddModelError("", "Something went wrong while saving");
                    return StatusCode(500, ModelState);
                }

                return Ok("Successfully created");
            }
            [HttpPost("login")]
            [ProducesResponseType(200)]
            [ProducesResponseType(400)]
            public async Task<IActionResult> Login(CreateUserDto request)
            {
                if (request == null)
                {
                    return BadRequest(ModelState);
                }

                if (!await _UserRepository.UsernameExistsAsync(request.Username))
                {
                    return BadRequest("User with such name does not exist!");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                User user = _Mapper.Map<User>(await _UserRepository.GetUserByNameAsync(request.Username));

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

                SetRefreshToken(refreshToken);

                return Ok(token);
            }
            [HttpPost("refresh-token"), Authorize(Roles = "User,Admin")]
            [ProducesResponseType(200)]
            [ProducesResponseType(400)]
            public async Task<IActionResult> GetRefreshToken()
            {
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

                var refreshToken = Request.Cookies["refreshToken"];

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                User user = _Mapper.Map<User>(await _UserRepository.GetUserByNameAsync(username));

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

                if (!await _UserRepository.UpdateRefreshTokenToUserAsync(user, newRefreshToken))
                {
                    ModelState.AddModelError("", "Something went wrong while updating the refresh token");
                    return StatusCode(500, ModelState);
                }

                SetRefreshToken(newRefreshToken);

                return Ok(token);
            }
            private void SetRefreshToken(RefreshToken newRefreshToken)
            {
                CookieOptions cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = newRefreshToken.TokenExpires
                };

                Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
            }
        }
    }
