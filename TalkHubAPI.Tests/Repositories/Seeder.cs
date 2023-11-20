using FakeItEasy;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TalkHubAPI.Data;
using TalkHubAPI.Interfaces.ServiceInterfaces;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.MessengerModels;
using TalkHubAPI.Models.PhotosManagerModels;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Tests.Repositories
{
    public class Seeder
    {
        private readonly IAuthService _AuthService;

        public Seeder()
        {
            _AuthService = A.Fake<IAuthService>();
        }

        public TalkHubContext GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<TalkHubContext>().EnableSensitiveDataLogging(true)
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            TalkHubContext context = new TalkHubContext(options);

            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                UserPassword userPassword = A.Fake<UserPassword>();
                userPassword.PasswordHash = Encoding.UTF8.GetBytes("fakeHash");
                userPassword.PasswordSalt = Encoding.UTF8.GetBytes("fakeSalt");
                RefreshToken token = A.Fake<RefreshToken>();
                token.Id = 1;
                token.TokenCreated = DateTime.UtcNow;
                token.TokenExpires = DateTime.UtcNow.AddDays(1);
                token.Token = "fakeToken";

                A.CallTo(() => _AuthService.CreatePasswordHash("prototype")).Returns(userPassword);
                List<User> users = new List<User>()
                {
                    new User()
                    {
                        Username = "TOMAAAA",
                        PasswordHash = userPassword.PasswordHash,
                        PasswordSalt = userPassword.PasswordSalt,
                        PermissionType = UserRole.Admin,
                        Email = "tomaaa@gmail.com",
                        RefreshToken = token,
                        SubscriberCount = 0
                    },
                    new User()
                    {
                        Username = "KristiQn Enchev",
                        PasswordHash = userPassword.PasswordHash,
                        PasswordSalt = userPassword.PasswordSalt,
                        PermissionType = UserRole.User,
                        Email = "tomaaa@gmail.com",
                        RefreshToken = token,
                        SubscriberCount = 0
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();


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

                context.PhotoCategories.AddRange(photoCategories);
                context.SaveChanges();

                List<Photo> photos = new List<Photo>()
                {
                    new Photo()
                    {
                        FileName = "hidden.webp",
                        Timestamp = DateTime.Now,
                        Description = "CLUSTER CHARGE GOING LIVE",
                        UserId = context.Users.First().Id,
                        CategoryId = context.PhotoCategories.First().Id
                    },
                    new Photo()
                    {
                        FileName = "hidden.webp",
                        Timestamp = DateTime.Now,
                        Description = "lord tachanka",
                        UserId = context.Users.First().Id + 1,
                        CategoryId = context.PhotoCategories.First().Id + 1
                    }
                };

                context.Photos.AddRange(photos);
                context.SaveChanges();

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

                context.ForumThreads.AddRange(forumThreads);
                context.SaveChanges();

                List<ForumMessage> forumMessages = new List<ForumMessage>()
                {
                    new ForumMessage()
                    {
                        MessageContent = "Starting area glitch",
                        FileName = "hidden.webp",
                        UserId = context.Users.First().Id,
                        DateCreated = DateTime.Now,
                        ForumThreadId = context.ForumThreads.First().Id,
                        UpvoteCount = 1
                    },
                    new ForumMessage()
                    {
                        MessageContent = "Good one",
                        FileName = "hidden.webp",
                        UserId = context.Users.First().Id + 1,
                        DateCreated = DateTime.Now,
                        ForumThreadId = context.ForumThreads.First().Id,
                        UpvoteCount = 0
                    },
                };

                context.ForumMessages.AddRange(forumMessages);
                context.SaveChanges();

                List<UserUpvote> userUpvotes = new List<UserUpvote>()
                {
                    new UserUpvote()
                    {
                        UserId = context.Users.First().Id,
                        MessageId = context.ForumMessages.First().Id,
                        Rating = 1,
                    }
                };

                context.UserUpvotes.AddRange(userUpvotes);
                context.SaveChanges();

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

                context.MessageRooms.AddRange(rooms);
                context.SaveChanges();

                List<MessengerMessage> messengerMessages = new List<MessengerMessage>()
                {
                    new MessengerMessage()
                    {
                        UserId = context.Users.First().Id,
                        RoomId = context.MessageRooms.First().Id,
                        MessageContent = "hi",
                        DateCreated = DateTime.Now,
                    },
                    new MessengerMessage()
                    {
                        UserId = context.Users.First().Id + 1,
                        RoomId = context.MessageRooms.First().Id + 1,
                        MessageContent = "hey",
                        DateCreated = DateTime.Now,
                    }
                };

                context.MessengerMessages.AddRange(messengerMessages);
                context.SaveChanges();

                List<UserMessageRoom> userMessageRooms = new List<UserMessageRoom>()
                {
                    new UserMessageRoom()
                    {
                        UserId = context.Users.First().Id,
                        RoomId = context.MessageRooms.First().Id,
                    },
                    new UserMessageRoom()
                    {
                        UserId = context.Users.First().Id + 1,
                        RoomId = context.MessageRooms.First().Id + 1,
                    }
                };

                context.UserMessageRooms.AddRange(userMessageRooms);
                context.SaveChanges();

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

                context.VideoTags.AddRange(videoTags);
                context.SaveChanges();

                List<Video> videos = new List<Video>()
                {
                    new Video()
                    {
                        VideoName = "The story of Geralt of Rivia",
                        Mp4name = "hidden_video.webm",
                        ThumbnailName = "hidden.png",
                        VideoDescription = "The full story",
                        LikeCount = 0,
                        UserId = context.Users.First().Id,
                        TagId = context.VideoTags.First().Id
                    },
                    new Video()
                    {
                        VideoName = "Death note lore",
                        Mp4name = "hidden_video.webm",
                        ThumbnailName = "hidden.png",
                        VideoDescription = "The full story",
                        LikeCount = 0,
                        UserId = context.Users.First().Id + 1,
                        TagId = context.VideoTags.First().Id
                    }
                };

                context.Videos.AddRange(videos);
                context.SaveChanges();

                List<VideoComment> videoComments = new List<VideoComment>()
                {
                    new VideoComment()
                    {
                        MessageContent = "nice video",
                        DateCreated = DateTime.Now,
                        LikeCount = 0,
                        UserId = context.Users.First().Id,
                        VideoId = context.Videos.First().Id,
                    },
                    new VideoComment()
                    {
                        MessageContent = "wow",
                        DateCreated = DateTime.Now,
                        LikeCount = 0,
                        UserId = context.Users.First().Id,
                        VideoId = context.Videos.First().Id,
                    }
                };

                context.VideoComments.AddRange(videoComments);
                context.SaveChanges();

                List<Playlist> playlists = new List<Playlist>()
                {
                    new Playlist()
                    {
                        PlaylistName = "funny",
                        UserId = context.Users.First().Id,
                    },
                    new Playlist()
                    {
                        PlaylistName = "television",
                        UserId= context.Users.First().Id + 1,
                    }
                };

                context.Playlists.AddRange(playlists);
                context.SaveChanges();

                List<VideoPlaylist> videoPlaylists = new List<VideoPlaylist>()
                {
                    new VideoPlaylist()
                    {
                        PlaylistId = context.Playlists.First().Id,
                        VideoId = context.Videos.First().Id
                    },
                    new VideoPlaylist()
                    {
                        PlaylistId = context.Playlists.First().Id,
                        VideoId= context.Videos.First().Id + 1
                    }
                };

                context.VideoPlaylists.AddRange(videoPlaylists);
                context.SaveChanges();

                List<VideoCommentsLike> videoCommentsLikes = new List<VideoCommentsLike>()
                {
                    new VideoCommentsLike()
                    {
                        UserId = context.Users.First().Id,
                        VideoCommentId = context.VideoComments.First().Id,
                        Rating = 1,
                    }
                };

                context.VideoCommentsLikes.AddRange(videoCommentsLikes);
                context.SaveChanges();

                List<VideoUserLike> videoUserLikes = new List<VideoUserLike>()
                {
                    new VideoUserLike()
                    {
                        UserId = context.Users.First().Id,
                        VideoId = context.Videos.First().Id,
                        Rating = 1,
                    }
                };

                context.VideoUserLikes.AddRange(videoUserLikes);
                context.SaveChanges();
            }

            return context;
        }
    }
}