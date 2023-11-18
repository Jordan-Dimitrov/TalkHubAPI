using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalkHubAPI.Data;
using TalkHubAPI.Dtos;
using TalkHubAPI.Helper;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Repositories;
using TalkHubAPI.Tests.Repositories;

namespace TalkHubAPI.Tests.Repository
{
    public class UserRepositoryTests
    {
        private readonly IAuthService _AuthService;
        private readonly Seeder _Seeder;
        public UserRepositoryTests()
        {
            _AuthService = A.Fake<IAuthService>();
            _Seeder = new Seeder();
        }

        [Fact]
        public async Task UserRepository_GetUserByNameAsync_ReturnsUser()
        {
            string name = "TOMAAAA";
            TalkHubContext context = _Seeder.GetDatabaseContext();
            UserRepository userRepository= new UserRepository(context);

            User result = await userRepository.GetUserByNameAsync(name);

            result.Should().NotBeNull();
            result.Should().BeOfType<User>();
        }

        [Fact]
        public async Task UserRepository_GetUserByRefreshTokenAsync_ReturnsUser()
        {
            string refreshToken = "fakeToken";
            TalkHubContext context = _Seeder.GetDatabaseContext();
            UserRepository userRepository = new UserRepository(context);

            User result = await userRepository.GetUserByRefreshTokenAsync(refreshToken);

            result.Should().NotBeNull();
            result.Should().BeOfType<User>();
        }

        [Fact]
        public async Task UserRepository_GetUserAsync_ReturnsUser()
        {
            int userId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            UserRepository userRepository = new UserRepository(context);

            User result = await userRepository.GetUserAsync(userId);

            result.Should().NotBeNull();
            result.Should().BeOfType<User>();
        }

        [Fact]
        public async Task UserRepository_GetUsesrAsync_ReturnsUser()
        {
            TalkHubContext context = _Seeder.GetDatabaseContext();
            UserRepository userRepository = new UserRepository(context);

            ICollection<User> result = await userRepository.GetUsersAsync();

            result.Should().NotBeNull();
            result.Should().AllBeOfType<User>();
        }

        [Fact]
        public async Task UserRepository_SaveAsync_ReturnsTrue()
        {
            TalkHubContext context = _Seeder.GetDatabaseContext();
            UserRepository userRepository = new UserRepository(context);

            bool result = await userRepository.SaveAsync();

            result.Should().BeFalse();
        }

        [Fact]
        public async Task UserRepository_CreateUserAsync_ReturnsTrue()
        {
            User user = A.Fake<User>();
            UserPassword userPassword = A.Fake<UserPassword>();
            userPassword.PasswordHash = Encoding.UTF8.GetBytes("fakeHash");
            userPassword.PasswordSalt = Encoding.UTF8.GetBytes("fakeSalt");
            user.PasswordHash = userPassword.PasswordHash;
            user.PasswordSalt = userPassword.PasswordSalt;
            user.Username = "fakeName";
            user.Email = "fakeMail";
            TalkHubContext context = _Seeder.GetDatabaseContext();
            UserRepository userRepository = new UserRepository(context);

            bool result = await userRepository.CreateUserAsync(user);

            User createdUser = await context.Users.FindAsync(user.Id);
            createdUser.Should().NotBeNull();
            createdUser.Username.Should().Be("fakeName");
            result.Should().BeTrue();
        }

        /*[Fact]
        public async Task UserRepository_DeleteUserAsync_ReturnsTrue()
        {
            TalkHubContext context = _Seeder.GetDatabaseContext();
            User user = await context.Users.FirstOrDefaultAsync();
            UserRepository userRepository = new UserRepository(context);

            bool result = await userRepository.DeleteUserAsync(user);

            User createdUser = await context.Users.FindAsync(user.Id);
            createdUser.Should().BeNull();
            result.Should().BeTrue();
        }*/

        [Fact]
        public async Task UserRepository_UpdateUserAsync_ReturnsTrue()
        {
            TalkHubContext context = _Seeder.GetDatabaseContext();
            User user = await context.Users.FirstOrDefaultAsync();
            UserRepository userRepository = new UserRepository(context);
            user.Username = "fakeNewName";

            bool result = await userRepository.UpdateUserAsync(user);

            User updatedUser = await context.Users.FindAsync(user.Id);
            updatedUser.Should().NotBeNull();
            updatedUser.Username.Should().Be("fakeNewName");
            result.Should().BeTrue();
        }

        [Fact]
        public async Task UserRepository_UserExistsAsync_ReturnsTrue()
        {
            int userId = 1;
            TalkHubContext context = _Seeder.GetDatabaseContext();
            UserRepository userRepository = new UserRepository(context);

            bool result = await userRepository.UserExistsAsync(userId);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task UserRepository_UsernameExistsAsync_ReturnsTrue()
        {
            string username = "TOMAAAA";
            TalkHubContext context = _Seeder.GetDatabaseContext();
            UserRepository userRepository = new UserRepository(context);

            bool result = await userRepository.UsernameExistsAsync(username);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task UserRepository_UpdateRefreshTokenToUserAsync_ReturnsTrue()
        {
            TalkHubContext context = _Seeder.GetDatabaseContext();
            User user = await context.Users.FirstOrDefaultAsync();
            RefreshToken token = A.Fake<RefreshToken>();
            token.Token = "fakeToken";
            token.TokenCreated = DateTime.UtcNow;
            token.TokenExpires = DateTime.UtcNow.AddDays(1);
            UserRepository userRepository = new UserRepository(context);
            user.RefreshToken = token;

            bool result = await userRepository.UpdateUserAsync(user);

            User updatedUser = await context.Users.FindAsync(user.Id);
            updatedUser.Should().NotBeNull();
            updatedUser.RefreshToken.Should().Be(token);
            result.Should().BeTrue();
        }
    }
}
