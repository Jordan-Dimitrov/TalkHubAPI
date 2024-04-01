using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TalkHubAPI.Dtos.UserDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ConfigurationModels;

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
        private readonly IMailService _MailService;
        private readonly PasswordResetTokenSettings _PasswordResetTokenSettings;
        private readonly MemoryCacheSettings _MemoryCacheSettings;
        public UserController(IUserRepository userRepository,
            IMapper mapper,
            IAuthService authService,
            IMemoryCache memoryCache,
            IMailService mailService,
            IOptions<PasswordResetTokenSettings> passwordResetTokenSettings,
            IOptions<MemoryCacheSettings> memoryCacheSettings)
        {
            _UserRepository = userRepository;
            _Mapper = mapper;
            _AuthService = authService;
            _MemoryCache = memoryCache;
            _UserCacheKey = "users";
            _MailService = mailService;
            _PasswordResetTokenSettings = passwordResetTokenSettings.Value;
            _MemoryCacheSettings = memoryCacheSettings.Value;
        }

        [HttpGet, Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        public async Task<IActionResult> GetUsers()
        {
            ICollection<UserDto>? users = _MemoryCache.Get<List<UserDto>>(_UserCacheKey);

            if (users is null)
            {
                users = _Mapper.Map<List<UserDto>>(await _UserRepository.GetUsersAsync());

                _MemoryCache.Set(_UserCacheKey, users, TimeSpan.FromHours(_MemoryCacheSettings.HoursExpiry));
            }

            return Ok(users);
        }

        [HttpGet("subscribtions"), Authorize(Roles = "User,Admin")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
        public async Task<IActionResult> GetUserSubscribtions()
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

            ICollection<UserDto>? users = _Mapper.Map<List<UserDto>>(await _UserRepository
                .GetUserChannelSubscribtions(user.Id));

            return Ok(users);
        }

        [HttpGet("{userId}"), Authorize(Roles = "User,Admin")]
        [ResponseCache(CacheProfileName = "Default")]
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

        [HttpPut("hide/{userId}"), Authorize(Roles = "Admin")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            User? user = await _UserRepository.GetUserAsync(userId);

            if (user is null)
            {
                return NotFound();
            }

            user.PermissionType = UserRole.Visitor;
            user.Username = "removed user";
            user.PasswordHash = new byte[2];
            user.PasswordSalt = new byte[2];
            user.RefreshToken = null;
            user.Email = "removed";
            user.VerificationToken = null;
            user.RefreshToken = null;

            if (!await _UserRepository.UpdateUserAsync(user))
            {
                ModelState.AddModelError("", "Something went wrong updating the user");
                return StatusCode(500, ModelState);
            }

            _MemoryCache.Remove(_UserCacheKey);

            return NoContent();
        }

        [HttpPost("register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto request)
        {
            if (request is null)
            {
                return BadRequest(ModelState);
            }

            if (await _UserRepository.UsernameExistsAsync(request.Username)
                || await _UserRepository.EmailExistsAsync(request.Email))
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
            user.PermissionType = UserRole.Visitor;
            user.VerificationToken = _AuthService.CreateRandomToken();
            user.SubscriberCount = 0;

            MailData mailData = new MailData();
            mailData.EmailToId = request.Email;
            mailData.EmailToName = request.Username;
            mailData.EmailSubject = "Accout verification";
            string host = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            mailData.EmailBody = $"Verify your account by clicking the provided link - {host}" + $"/api/User/verify/{user.VerificationToken}";

            if (!await _UserRepository.CreateUserAsync(user) || !_MailService.SendMail(mailData))
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
        [ResponseCache(CacheProfileName = "Default")]
        public async Task<IActionResult> GetUserRole()
        {
            string? jwtToken = Request.Cookies["jwtToken"];
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
        public async Task<IActionResult> Login(LoginUserDto request)
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

            if (user.PermissionType == UserRole.Visitor)
            {
                return BadRequest("User not verified!");
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
            _AuthService.SetJwtToken(token);

            return Ok("Logged in successfully");
        }

        [HttpGet("verify/{token}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Verify(string token)
        {
            User? user = await _UserRepository.GetUserByVerificationTokenAsync(token);

            if (user is null)
            {
                return BadRequest("Invalid token.");
            }

            user.VerifiedAt = DateTime.Now;
            user.PermissionType = UserRole.User;
            user.VerificationToken = null;

            if (!await _UserRepository.UpdateUserAsync(user))
            {
                ModelState.AddModelError("", "Something went wrong while updating the user");
                return StatusCode(500, ModelState);
            }

            RefreshToken refreshToken = _AuthService.GenerateRefreshToken();

            //there should be a redirect response
            return Ok("User verified");
        }

        [HttpPost("forgot-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            User? user = await _UserRepository.GetUserByEmailAsync(email);

            if (user is null)
            {
                return BadRequest("User with such email does not exist!");
            }

            user.PasswordResetToken = _AuthService.CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddMinutes(_PasswordResetTokenSettings.MinutesExpiry);

            MailData mailData = new MailData();
            mailData.EmailToId = user.Email;
            mailData.EmailToName = user.Username;
            mailData.EmailSubject = "Reset password";

            //the email body should contain a url to the client that handles the password reset
            mailData.EmailBody = user.PasswordResetToken;

            if (!await _UserRepository.UpdateUserAsync(user) || !_MailService.SendMail(mailData))
            {
                ModelState.AddModelError("", "Something went wrong while updating the user");
                return StatusCode(500, ModelState);
            }

            return Ok("You may now reset your password");
        }

        [HttpPost("reset-password")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ResetPassword(UserResetPasswordDto request)
        {
            User? user = await _UserRepository.GetUserByPasswordResetTokenAsync(request.Token);

            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserPassword pass = _AuthService.CreatePasswordHash(request.Password);

            user.PasswordHash = pass.PasswordHash;
            user.PasswordSalt = pass.PasswordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            if (!await _UserRepository.UpdateUserAsync(user))
            {
                ModelState.AddModelError("", "Something went wrong while updating the user");
                return StatusCode(500, ModelState);
            }

            return Ok("Password successfully reset.");
        }

        [HttpPost("logout")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Logout()
        {
            string? refreshToken = Request.Cookies["refreshToken"];
            string? jwtToken = Request.Cookies["jwtToken"];

            if (refreshToken.IsNullOrEmpty() && jwtToken.IsNullOrEmpty())
            {
                return BadRequest("No tokens to log out");
            }

            User? user = await _UserRepository.GetUserByRefreshTokenAsync(refreshToken);

            if (user is null)
            {
                return BadRequest("Invalid user");
            }

            RefreshToken token = _AuthService.GenerateRefreshToken();

            await _UserRepository.UpdateRefreshTokenToUserAsync(user, token);

            _AuthService.ClearTokens();
            return Ok("Logged out successfully");
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
            _AuthService.SetJwtToken(token);

            return Ok();
        }
    }
}
