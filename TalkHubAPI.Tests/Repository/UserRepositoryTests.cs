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
                //This is not faked and the test will not pass
                _AuthService.CreatePasswordHash("prototype", out byte[] passwordHash, out byte[] passwordSalt);
                List<User> users = new List<User>()
                {
                    new User()
                    {
                        Username = "TOMAAAA",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        RefreshToken = _AuthService.GenerateRefreshToken()
                    },
                    new User()
                    {
                        Username = "KristiQn Enchev",
                        PasswordHash = passwordHash,
                        PasswordSalt = passwordSalt,
                        RefreshToken = _AuthService.GenerateRefreshToken()
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            return context;
        }

        /*[Fact]
        public void UserRepository_GetUser_ReturnsUser()
        {
            string name = "TOMAAAA";
            TalkHubContext context = GetDatabaseContext();
            UserRepository userRepository= new UserRepository(context);

            User result = userRepository.GetUserByName(name);

            result.Should().NotBeNull();
            result.Should().BeOfType<User>();
        }
        */
        //TODO: Add a test for every method and fix the GetDatabaseContext method
    }
}
