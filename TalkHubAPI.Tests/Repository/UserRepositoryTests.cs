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
using TalkHubAPI.Dto;
using TalkHubAPI.Helper;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Repository;

namespace TalkHubAPI.Tests.Repository
{
    public class UserRepositoryTests
    {
        private readonly IAuthService _AuthService;
        public UserRepositoryTests()
        {
            _AuthService = A.Fake<IAuthService>();
        }
        private TalkHubContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<TalkHubContext>().EnableSensitiveDataLogging(true)
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            TalkHubContext context = new TalkHubContext(options);
            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                var userPassword = A.Fake<UserPassword>();
                userPassword.PasswordHash = Encoding.UTF8.GetBytes("fakeHash");
                userPassword.PasswordSalt = Encoding.UTF8.GetBytes("fakeSalt");

                A.CallTo(() => _AuthService.CreatePasswordHash("prototype")).Returns(userPassword);//This is not faked and the test will not pass
                List<User> users = new List<User>()
                {
                    new User()
                    {
                        Username = "TOMAAAA",
                        PasswordHash = userPassword.PasswordHash,
                        PasswordSalt = userPassword.PasswordSalt,
                    },
                    new User()
                    {
                        Username = "KristiQn Enchev",
                        PasswordHash = userPassword.PasswordHash,
                        PasswordSalt = userPassword.PasswordSalt,
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            return context;
        }

        [Fact]
        public async Task UserRepository_GetUserAsync_ReturnsUser()
        {
            string name = "TOMAAAA";
            TalkHubContext context = GetDatabaseContext();
            UserRepository userRepository= new UserRepository(context);

            User result = await userRepository.GetUserByNameAsync(name);

            result.Should().NotBeNull();
            result.Should().BeOfType<User>();
        }
        //TODO: Add a test for every method and fix the GetDatabaseContext method
    }
}
