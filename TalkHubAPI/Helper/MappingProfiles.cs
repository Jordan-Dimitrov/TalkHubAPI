using AutoMapper;
using TalkHubAPI.Dto;
using TalkHubAPI.Models;

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
        }
    }
}
