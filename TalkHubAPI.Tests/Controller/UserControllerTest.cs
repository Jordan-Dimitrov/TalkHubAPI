﻿using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkHubAPI.Controllers;
using TalkHubAPI.Dto.UserDtos;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;

namespace TalkHubAPI.Tests.Controller
{
    public class UserControllerTest
    {
        private readonly IUserRepository _UserRepository;
        private readonly IMapper _Mapper;
        private readonly IAuthService _AuthService;
        public UserControllerTest()
        {
            _UserRepository = A.Fake<IUserRepository>();
            _Mapper = A.Fake<IMapper>();
            _AuthService = A.Fake<IAuthService>();
        }

        [Fact]
        public async Task UserController_GetUsers_ReturnOk()
        {
            ICollection<UserDto> users = A.Fake<ICollection<UserDto>>();
            List<UserDto> usersList = A.Fake<List<UserDto>>();

            A.CallTo(() => _Mapper.Map<List<UserDto>>(users)).Returns(usersList);
            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService);

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
            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService);

            IActionResult result = await controller.GetUser(userId);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UserController_Register_ReturnOk()
        {
            CreateUserDto userDto = A.Fake<CreateUserDto>();
            User user = A.Fake<User>();
            UserPassword pass = A.Fake<UserPassword>();
            pass.PasswordHash = Encoding.UTF8.GetBytes("fakeHash");
            pass.PasswordSalt = Encoding.UTF8.GetBytes("fakeSalt");

            A.CallTo(() => _UserRepository.UsernameExistsAsync(userDto.Username)).Returns(false);
            A.CallTo(() => _Mapper.Map<User>(userDto)).Returns(user);
            A.CallTo(() => _AuthService.CreatePasswordHash("fakePass")).Returns(pass);
            A.CallTo(() => _UserRepository.CreateUserAsync(user)).Returns(true);
            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService);

            IActionResult result = await controller.Register(userDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UserController_Login_ReturnOk()
        {
            CreateUserDto userDto = A.Fake<CreateUserDto>();
            User user = A.Fake<User>();
            UserPassword pass = A.Fake<UserPassword>();
            user.PasswordHash = Encoding.UTF8.GetBytes("fakeHash");
            user.PasswordSalt = Encoding.UTF8.GetBytes("fakeSalt");

            RefreshToken token = A.Fake<RefreshToken>();
            token.Token = "fakeToken";
            token.TokenCreated = DateTime.UtcNow;
            token.TokenExpires = DateTime.UtcNow.AddDays(1);

            A.CallTo(() => _UserRepository.UsernameExistsAsync(userDto.Username)).Returns(true);
            A.CallTo(() => _UserRepository.GetUserByNameAsync(userDto.Username)).Returns(user);
            A.CallTo(() => _Mapper.Map<User>(userDto)).Returns(user);
            A.CallTo(() => _AuthService.VerifyPasswordHash("fakePass", user.PasswordHash, user.PasswordSalt)).Returns(true);
            A.CallTo(() => _AuthService.GenerateJwtToken(user)).Returns("fakeJwtToken");
            A.CallTo(() => _AuthService.GenerateRefreshToken()).Returns(token);
            A.CallTo(() => _UserRepository.UpdateRefreshTokenToUserAsync(user, token)).Returns(true);
            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService);

            IActionResult result = await controller.Login(userDto);

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }

        /*[Fact]
        public async Task UserController_GetRefreshToken_ReturnOk()
        {
            User user = A.Fake<User>();
            RefreshToken token = A.Fake<RefreshToken>();
            token.Token = "fakeToken";
            token.TokenCreated = DateTime.UtcNow;
            token.TokenExpires = DateTime.UtcNow.AddDays(1);

            A.CallTo(() => _AuthService.GetUsernameFromJwtToken("fakeToken")).Returns(user.Username);
            A.CallTo(() => _UserRepository.UsernameExistsAsync(user.Username)).Returns(true);
            A.CallTo(() => _UserRepository.GetUserByNameAsync(user.Username)).Returns(user);
            A.CallTo(() => _Mapper.Map<User>(user)).Returns(user);
            A.CallTo(() => _AuthService.GenerateJwtToken(user)).Returns("fakeJwtToken");
            A.CallTo(() => _AuthService.GenerateRefreshToken()).Returns(token);
            A.CallTo(() => _UserRepository.UpdateRefreshTokenToUserAsync(user, token)).Returns(true);
            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService);

            IActionResult result = await controller.GetRefreshToken();

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }*/
    }
}
