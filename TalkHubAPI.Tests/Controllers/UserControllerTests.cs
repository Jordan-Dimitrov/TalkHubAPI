﻿using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text;
using TalkHubAPI.Controllers;
using TalkHubAPI.Dtos.UserDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ConfigurationModels;

namespace TalkHubAPI.Tests.Controller
{
    public class UserControllerTests
    {
        private readonly IUserRepository _UserRepository;
        private readonly IMapper _Mapper;
        private readonly IAuthService _AuthService;
        private readonly IMemoryCache _MemoryCache;
        private readonly IMailService _MailService;
        private readonly IOptions<PasswordResetTokenSettings> _PasswordResetTokenSettings;
        private readonly IOptions<MemoryCacheSettings> _MemoryCacheSettings;
        public UserControllerTests()
        {
            _UserRepository = A.Fake<IUserRepository>();
            _Mapper = A.Fake<IMapper>();
            _AuthService = A.Fake<IAuthService>();
            _MemoryCache = A.Fake<IMemoryCache>();
            _MailService = A.Fake<IMailService>();
            _PasswordResetTokenSettings = A.Fake<IOptions<PasswordResetTokenSettings>>();
            _MemoryCacheSettings = A.Fake<IOptions<MemoryCacheSettings>>();
        }

        [Fact]
        public async Task UserController_GetUsers_ReturnOk()
        {
            ICollection<UserDto> users = A.Fake<ICollection<UserDto>>();
            List<UserDto> usersList = A.Fake<List<UserDto>>();

            A.CallTo(() => _Mapper.Map<List<UserDto>>(users)).Returns(usersList);
            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService,
                _MemoryCache, _MailService, _PasswordResetTokenSettings, _MemoryCacheSettings);

            IActionResult result = await controller.GetUsers();

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UserController_GetUser_ReturnOk()
        {
            int userId = 1;
            UserDto userDto = A.Fake<UserDto>();
            User user = A.Fake<User>();
            A.CallTo(() => _UserRepository.UserExistsAsync(userId)).Returns(true);
            A.CallTo(() => _UserRepository.GetUserAsync(userId)).Returns(user);
            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService,
                _MemoryCache, _MailService, _PasswordResetTokenSettings, _MemoryCacheSettings);

            IActionResult result = await controller.GetUser(userId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UserController_Register_ReturnOk()
        {
            RegisterUserDto userDto = A.Fake<RegisterUserDto>();
            User user = A.Fake<User>();
            UserPassword pass = A.Fake<UserPassword>();
            pass.PasswordHash = Encoding.UTF8.GetBytes("fakeHash");
            pass.PasswordSalt = Encoding.UTF8.GetBytes("fakeSalt");

            A.CallTo(() => _UserRepository.UsernameExistsAsync(userDto.Username)).Returns(false);
            A.CallTo(() => _Mapper.Map<User>(userDto)).Returns(user);
            A.CallTo(() => _AuthService.CreatePasswordHash("fakePass")).Returns(pass);
            A.CallTo(() => _UserRepository.CreateUserAsync(user)).Returns(true);
            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService,
                _MemoryCache, _MailService, _PasswordResetTokenSettings, _MemoryCacheSettings);

            IActionResult result = await controller.Register(userDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UserController_Login_ReturnOk()
        {
            HttpResponse httpResponseFake = A.Fake<HttpResponse>();
            LoginUserDto userDto = A.Fake<LoginUserDto>();
            userDto.Password = "fakeHash";
            User user = A.Fake<User>();
            UserPassword pass = A.Fake<UserPassword>();
            user.PasswordHash = Encoding.UTF8.GetBytes("fakeHash");
            user.PasswordSalt = Encoding.UTF8.GetBytes("fakeSalt");
            user.PermissionType = UserRole.User;

            RefreshToken token = A.Fake<RefreshToken>();
            token.Token = "fakeToken";
            token.TokenCreated = DateTime.UtcNow;
            token.TokenExpires = DateTime.UtcNow.AddDays(1);

            A.CallTo(() => _UserRepository.UsernameExistsAsync(userDto.Username)).Returns(true);
            A.CallTo(() => _UserRepository.GetUserByNameAsync(userDto.Username)).Returns(user);
            A.CallTo(() => _Mapper.Map<User>(userDto)).Returns(user);
            A.CallTo(() => _AuthService.VerifyPasswordHash("fakeHash", user.PasswordHash, user.PasswordSalt)).Returns(true);
            A.CallTo(() => _AuthService.GenerateJwtToken(user)).Returns("fakeJwtToken");
            A.CallTo(() => _AuthService.GenerateRefreshToken()).Returns(token);
            A.CallTo(() => _UserRepository.UpdateRefreshTokenToUserAsync(user, token)).Returns(true);
            A.CallTo(() => _AuthService.SetRefreshToken(token));
            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService,
                _MemoryCache, _MailService, _PasswordResetTokenSettings, _MemoryCacheSettings);

            IActionResult result = await controller.Login(userDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UserController_GetUserRole_ReturnOk()
        {
            User user = A.Fake<User>();
            user.Username = "TOMAAA";

            HttpContext httpContext = A.Fake<HttpContext>();
            HttpRequest fakeRequest = A.Fake<HttpRequest>();

            ControllerContext controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            A.CallTo(() => fakeRequest.Cookies["jwtToken"]).Returns("fakeToken");
            A.CallTo(() => httpContext.Request).Returns(fakeRequest);
            A.CallTo(() => _AuthService.GetUsernameFromJwtToken("fakeToken")).Returns(user.Username);
            A.CallTo(() => _AuthService.GetRoleFromJwtToken("fakeToken")).Returns("Admin");
            A.CallTo(() => _UserRepository.UsernameExistsAsync(user.Username)).Returns(true);

            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService,
                _MemoryCache, _MailService, _PasswordResetTokenSettings, _MemoryCacheSettings)
            {
                ControllerContext = controllerContext
            };

            IActionResult result = await controller.GetUserRole();

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void UserController_Logout_ReturnOk()
        {
            HttpContext httpContext = A.Fake<HttpContext>();
            HttpRequest fakeRequest = A.Fake<HttpRequest>();

            ControllerContext controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            A.CallTo(() => fakeRequest.Cookies["jwtToken"]).Returns("fakeToken");
            A.CallTo(() => fakeRequest.Cookies["refreshToken"]).Returns("fakeToken");
            A.CallTo(() => httpContext.Request).Returns(fakeRequest);
            A.CallTo(() => _AuthService.ClearTokens());

            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService,
                _MemoryCache, _MailService, _PasswordResetTokenSettings, _MemoryCacheSettings)
            {
                ControllerContext = controllerContext
            };

            var result = controller.Logout();

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UserController_GetRefreshToken_ReturnOk()
        {
            User user = A.Fake<User>();
            RefreshToken token = A.Fake<RefreshToken>();
            token.Token = "fakeToken";
            token.TokenCreated = DateTime.UtcNow;
            token.TokenExpires = DateTime.UtcNow.AddDays(1);
            user.RefreshToken = token;

            HttpContext httpContext = A.Fake<HttpContext>();
            HttpRequest fakeRequest = A.Fake<HttpRequest>();

            ControllerContext controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            A.CallTo(() => fakeRequest.Cookies["refreshToken"]).Returns("fakeToken");
            A.CallTo(() => httpContext.Request).Returns(fakeRequest);
            A.CallTo(() => _UserRepository.GetUserByRefreshTokenAsync("fakeToken")).Returns(user);
            A.CallTo(() => _AuthService.GenerateRefreshToken()).Returns(token);
            A.CallTo(() => _AuthService.GenerateJwtToken(user)).Returns("fakeJwtToken");
            A.CallTo(() => _UserRepository.UpdateRefreshTokenToUserAsync(user, token)).Returns(true);
            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService,
                _MemoryCache, _MailService, _PasswordResetTokenSettings, _MemoryCacheSettings)
            {
                ControllerContext = controllerContext
            };

            IActionResult result = await controller.GetRefreshToken();

            result.Should().BeOfType<OkResult>();
        }
    }
}
