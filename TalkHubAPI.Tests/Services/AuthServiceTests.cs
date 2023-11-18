using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkHubAPI.Helper;
using TalkHubAPI.Interfaces.ServiceInterfaces;

namespace TalkHubAPI.Tests.Services
{
    public class AuthServiceTests
    {
        private readonly IAuthService _AuthService;
        public AuthServiceTests() 
        {
            _AuthService = A.Fake<IAuthService>();
        }

        [Fact]
        public async Task AuthService_GetUsernameFromJwtToken_ReturnsString()
        {
            string jwtToken = "fakeToken";
            string username = "fakeName";
            A.CallTo(() => _AuthService.GetUsernameFromJwtToken(jwtToken)).Returns(username);

            string result = _AuthService.GetUsernameFromJwtToken(jwtToken);

            result.Should().NotBeNull();
        }
    }
}
