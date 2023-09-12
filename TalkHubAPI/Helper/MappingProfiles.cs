﻿using AutoMapper;
using TalkHubAPI.Dto.ForumDtos;
using TalkHubAPI.Dto.PhotosDtos;
using TalkHubAPI.Dto.UserDtos;
using TalkHubAPI.Models;
using TalkHubAPI.Models.ForumModels;
using TalkHubAPI.Models.PhotosManagerModels;

namespace TalkHubAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, CreateUserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
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
        }
    }
}
