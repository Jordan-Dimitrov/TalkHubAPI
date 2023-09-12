using System;
using System.Collections.Generic;
using System.Linq;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.PhotosManagerModels;

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

                List<ForumThread> forumThreads = new List<ForumThread>()
                {
                    new ForumThread()
                    {
                        ThreadName = "BOTW discussion",
                        ThreadDescription = "A place to discuss fun moments from the BOTW game"
                    },
                    new ForumThread()
                    {
                        ThreadName = "Jojo discussion",
                        ThreadDescription = "A place to discuss fun moments from the Jojo anime"
                    }
                };

                _Context.ForumThreads.AddRange(forumThreads);
                _Context.SaveChanges();

                List<ForumMessage> forumMessages = new List<ForumMessage>()
                {
                    new ForumMessage()
                    {
                        MessageContent = "Starting area glitch",
                        FileName = "link.png",
                        UserId = _Context.Users.First().Id,
                        DateCreated = DateTime.Now,
                        ForumThreadId = _Context.ForumThreads.First().Id,
                        UpvoteCount = 1
                    },
                    new ForumMessage()
                    {
                        MessageContent = "Good one",
                        FileName = "jotaro.png",
                        UserId = _Context.Users.First().Id + 1,
                        DateCreated = DateTime.Now,
                        ForumThreadId = _Context.ForumThreads.First().Id,
                        UpvoteCount = 0
                    },
                };

                _Context.ForumMessages.AddRange(forumMessages);
                _Context.SaveChanges();

                List<UserUpvote> userUpvotes = new List<UserUpvote>()
                {
                    new UserUpvote()
                    {
                        UserId = _Context.Users.First().Id,
                        MessageId = _Context.ForumMessages.First().Id,
                        Rating = 1,
                    }
                };

                _Context.UserUpvotes.AddRange(userUpvotes);
                _Context.SaveChanges();
            }
        }
    }
}
