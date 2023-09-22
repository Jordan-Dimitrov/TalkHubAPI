using AutoMapper;
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
            var users = A.Fake<ICollection<UserDto>>();
            var usersList = A.Fake<List<UserDto>>();

            A.CallTo(() => _Mapper.Map<List<UserDto>>(users)).Returns(usersList);
            UserController controller = new UserController(_UserRepository, _Mapper, _AuthService);

            IActionResult result = await controller.GetUsers();

            result.Should().NotBeNull();
            result.Should().BeOfType<OkObjectResult>();
        }
        //TODO: Add a test for every method
    }
}
