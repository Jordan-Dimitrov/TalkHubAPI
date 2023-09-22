using System;
using System.Collections.Generic;
using System.Linq;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.MessengerModels;
using TalkHubAPI.Models.PhotosManagerModels;
using TalkHubAPI.Models.VideoPlayerModels;

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
            UserPassword pass = _AuthService.CreatePasswordHash("prototype");

            if (!_Context.Users.Any())
            {
                List<User> users = new List<User>()
                {
                    new User()
                    {
                        Username = "TOMAAAA",
                        PasswordHash = pass.PasswordHash,
                        PasswordSalt = pass.PasswordSalt,
                        RefreshToken = _AuthService.GenerateRefreshToken(),
                        PermissionType = 1
                    },
                    new User()
                    {
                        Username = "KristiQn Enchev",
                        PasswordHash = pass.PasswordHash,
                        PasswordSalt = pass.PasswordSalt,
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
                        FileName = "hidden.webp",
                        Timestamp = DateTime.Now,
                        Description = "CLUSTER CHARGE GOING LIVE",
                        UserId = _Context.Users.First().Id,
                        CategoryId = _Context.PhotoCategories.First().Id
                    },
                    new Photo()
                    {
                        FileName = "hidden.webp",
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
                        ThreadDescription = "BOTW Related stuff"
                    },
                    new ForumThread()
                    {
                        ThreadName = "Jojo discussion",
                        ThreadDescription = "Jojo related stuff"
                    }
                };

                _Context.ForumThreads.AddRange(forumThreads);
                _Context.SaveChanges();

                List<ForumMessage> forumMessages = new List<ForumMessage>()
                {
                    new ForumMessage()
                    {
                        MessageContent = "Starting area glitch",
                        FileName = "hidden.webp",
                        UserId = _Context.Users.First().Id,
                        DateCreated = DateTime.Now,
                        ForumThreadId = _Context.ForumThreads.First().Id,
                        UpvoteCount = 1
                    },
                    new ForumMessage()
                    {
                        MessageContent = "Good one",
                        FileName = "hidden.webp",
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

                List<MessageRoom> rooms = new List<MessageRoom>()
                {
                    new MessageRoom()
                    {
                        RoomName = "chill room"
                    },
                    new MessageRoom()
                    {
                        RoomName = "sad room"
                    }
                };

                _Context.MessageRooms.AddRange(rooms);
                _Context.SaveChanges();

                List<MessengerMessage> messengerMessages = new List<MessengerMessage>()
                {
                    new MessengerMessage()
                    {
                        UserId = _Context.Users.First().Id,
                        RoomId = _Context.MessageRooms.First().Id,
                        MessageContent = "hi",
                        DateCreated = DateTime.Now,
                    },
                    new MessengerMessage()
                    {
                        UserId = _Context.Users.First().Id + 1,
                        RoomId = _Context.MessageRooms.First().Id + 1,
                        MessageContent = "hey",
                        DateCreated = DateTime.Now,
                    }
                };

                _Context.MessengerMessages.AddRange(messengerMessages);
                _Context.SaveChanges();

                List<UserMessageRoom> userMessageRooms = new List<UserMessageRoom>()
                {
                    new UserMessageRoom()
                    {
                        UserId = _Context.Users.First().Id,
                        RoomId = _Context.MessageRooms.First().Id,
                    },
                    new UserMessageRoom()
                    {
                        UserId = _Context.Users.First().Id + 1,
                        RoomId = _Context.MessageRooms.First().Id + 1,
                    }
                };

                _Context.UserMessageRooms.AddRange(userMessageRooms);
                _Context.SaveChanges();

                List<VideoTag> videoTags = new List<VideoTag>()
                {
                    new VideoTag()
                    {
                        TagName = "funny"
                    },
                    new VideoTag()
                    {
                        TagName = "television"
                    }
                };

                _Context.VideoTags.AddRange(videoTags);
                _Context.SaveChanges();

                List<Video> videos = new List<Video>()
                {
                    new Video()
                    {
                        VideoName = "The story of Geralt of Rivia",
                        Mp4name = "hidden_video.webm",
                        ThumbnailName = "hidden.png",
                        VideoDescription = "The full story",
                        LikeCount = 0,
                        UserId = _Context.Users.First().Id,
                        TagId = _Context.VideoTags.First().Id
                    },
                    new Video()
                    {
                        VideoName = "Death note lore",
                        Mp4name = "hidden_video.webm",
                        ThumbnailName = "hidden.png",
                        VideoDescription = "The full story",
                        LikeCount = 0,
                        UserId = _Context.Users.First().Id + 1,
                        TagId = _Context.VideoTags.First().Id
                    }
                };

                _Context.Videos.AddRange(videos);
                _Context.SaveChanges();

                List<VideoComment> videoComments = new List<VideoComment>()
                {
                    new VideoComment()
                    {
                        MessageContent = "nice video",
                        DateCreated = DateTime.Now,
                        LikeCount = 0,
                        UserId = _Context.Users.First().Id + 1,
                        VideoId = _Context.Videos.First().Id,
                    },
                    new VideoComment()
                    {
                        MessageContent = "wow",
                        DateCreated = DateTime.Now,
                        LikeCount = 0,
                        ReplyId = _Context.VideoComments.First().Id,
                        UserId = _Context.Users.First().Id,
                        VideoId = _Context.Videos.First().Id,
                    }
                };

                _Context.VideoComments.AddRange(videoComments);
                _Context.SaveChanges();

                List<Playlist> playlists = new List<Playlist>()
                {
                    new Playlist()
                    {
                        PlaylistName = "funny",
                        UserId = _Context.Users.First().Id,
                    },
                    new Playlist()
                    {
                        PlaylistName = "television",
                        UserId= _Context.Users.First().Id + 1,
                    }
                };

                _Context.Playlists.AddRange(playlists);
                _Context.SaveChanges();

                List<VideoPlaylist> videoPlaylists = new List<VideoPlaylist>()
                {
                    new VideoPlaylist()
                    {
                        PlaylistId = _Context.Playlists.First().Id,
                        VideoId= _Context.Videos.First().Id
                    },
                    new VideoPlaylist()
                    {
                        PlaylistId = _Context.Playlists.First().Id,
                        VideoId= _Context.Videos.First().Id + 1
                    }
                };

                _Context.VideoPlaylists.AddRange(videoPlaylists);
                _Context.SaveChanges();

                List<VideoCommentsLike> videoCommentsLikes = new List<VideoCommentsLike>()
                {
                    new VideoCommentsLike()
                    {
                        UserId = _Context.Users.First().Id,
                        VideoCommentId = _Context.VideoComments.First().Id,
                        Rating = 1,
                    }
                };

                _Context.VideoCommentsLikes.AddRange(videoCommentsLikes);
                _Context.SaveChanges();

                List<VideoUserLike> videoUserLikes = new List<VideoUserLike>()
                {
                    new VideoUserLike()
                    {
                        UserId = _Context.Users.First().Id,
                        VideoId = _Context.Videos.First().Id,
                        Rating = 1,
                    }
                };

                _Context.VideoUserLikes.AddRange(videoUserLikes);
                _Context.SaveChanges();
            }
        }
    }
}
