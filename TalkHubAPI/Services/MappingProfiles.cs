using AutoMapper;
using TalkHubAPI.Dtos.ForumDtos;
using TalkHubAPI.Dtos.MessengerDtos;
using TalkHubAPI.Dtos.PhotosDtos;
using TalkHubAPI.Dtos.UserDtos;
using TalkHubAPI.Dtos.VideoPlayerDtos;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.MessengerModels;
using TalkHubAPI.Models.PhotosManagerModels;
using TalkHubAPI.Models.VideoPlayerModels;

namespace TalkHubAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, RegisterUserDto>();
            CreateMap<RegisterUserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<User, LoginUserDto>();
            CreateMap<LoginUserDto, User>();
            CreateMap<User, UserResetPasswordDto>();
            CreateMap<UserResetPasswordDto, User>();

            CreateMap<PhotoCategory, PhotoCategoryDto>();
            CreateMap<PhotoCategoryDto, PhotoCategory>();
            CreateMap<Photo, CreatePhotoDto>();
            CreateMap<CreatePhotoDto, Photo>();
            CreateMap<Photo, PhotoDto>();
            CreateMap<PhotoDto, Photo>();

            CreateMap<ForumThread, ForumThreadDto>();
            CreateMap<ForumThreadDto, ForumThread>();
            CreateMap<ForumMessage, CreateForumMessageDto>();
            CreateMap<CreateForumMessageDto, ForumMessage>();
            CreateMap<ForumMessage,ForumMessageDto>();
            CreateMap<ForumMessageDto, ForumMessage>();
            CreateMap<UserUpvote, UserUpvoteDto>();
            CreateMap<UserUpvoteDto, UserUpvote>();

            CreateMap<MessageRoom, MessageRoomDto>();
            CreateMap<MessageRoomDto, MessageRoom>();
            CreateMap<MessengerMessage, MessengerMessageDto>();
            CreateMap<MessengerMessageDto, MessengerMessage>();
            CreateMap<MessengerMessage, SendMessengerMessageDto>();
            CreateMap<MessengerMessageDto, MessengerMessage>();
            CreateMap<MessengerMessage, SendMessengerMessageDto>();
            CreateMap<SendMessengerMessageDto, MessengerMessage>();

            CreateMap<VideoTag, VideoTagDto>();
            CreateMap<VideoTagDto, VideoTag>();
            CreateMap<Playlist, PlaylistDto>();
            CreateMap<PlaylistDto, Playlist>();
            CreateMap<Playlist, CreatePlaylistDto>();
            CreateMap<CreatePlaylistDto, Playlist>();

            CreateMap<Video, VideoDto>();
            CreateMap<VideoDto, Video>();
            CreateMap<Video, CreateVideoDto>();
            CreateMap<CreateVideoDto, Video>();

            CreateMap<VideoComment, VideoCommentDto>();
            CreateMap<VideoCommentDto, VideoComment>();
            CreateMap<VideoComment, CreateVideoCommentDto>().ReverseMap();
            CreateMap<CreateVideoCommentDto, VideoCommentDto>();

            CreateMap<VideoPlaylist, VideoPlaylistDto>();
            CreateMap<VideoPlaylistDto, VideoPlaylist>();

            CreateMap<UserSubscribtion, UserSubscribtionDto>();
            CreateMap<UserSubscribtionDto, UserSubscribtion>();
            CreateMap<VideoUserLike, VideoUserLikeDto>();
            CreateMap<VideoUserLikeDto, VideoUserLike>();
            CreateMap<VideoCommentsLike, VideoCommentsLikeDto>();
            CreateMap<VideoCommentsLikeDto, VideoCommentsLike>();
        }
    }
}
