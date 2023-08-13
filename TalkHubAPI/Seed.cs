using System;
using System.Collections.Generic;
using System.Linq;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;

namespace TalkHubAPI
{
    public class Seed
    {
        private readonly TalkHubContext _Context;
        private readonly IAuthService _AuthService;

        public Seed(TalkHubContext context, IAuthService authService)
        {
            _Context = context;
            _AuthService = authService;
        }

        public void SeedTalkHubContext()
        {
            _AuthService.CreatePasswordHash("prototype", out byte[] passwordHash, out byte[] passwordSalt);

            if (!_Context.Users.Any())
            {
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

                _Context.Users.AddRange(users);
                _Context.SaveChanges();

                List<PhotoCategory> photoCategories = new List<PhotoCategory>()
                {
                    new PhotoCategory()
                    {
                        CategoryName = "R6 attack"
                    },
                    new PhotoCategory()
                    {
                        CategoryName = "R6 defense"
                    }
                };

                _Context.PhotoCategories.AddRange(photoCategories);
                _Context.SaveChanges();

                List<Photo> photos = new List<Photo>()
                {
                    new Photo()
                    {
                        FileName = "fuze.png",
                        Timestamp = DateTime.Now,
                        Description = "CLUSTER CHARGE GOING LIVE",
                        UserId = _Context.Users.First().Id,
                        CategoryId = _Context.PhotoCategories.First().Id
                    },
                    new Photo()
                    {
                        FileName = "tachanka.png",
                        Timestamp = DateTime.Now,
                        Description = "lord tachanka",
                        UserId = _Context.Users.First().Id + 1,
                        CategoryId = _Context.PhotoCategories.First().Id + 1
                    }
                };

                _Context.Photos.AddRange(photos);
                _Context.SaveChanges();
            }
        }
    }
}
