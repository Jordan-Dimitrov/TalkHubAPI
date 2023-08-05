using AutoMapper;
using TalkHubAPI.Dto;
using TalkHubAPI.Models;

namespace TalkHubAPI.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDto>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => "********"));
            CreateMap<UserDto, User>();
        }
    }
}
