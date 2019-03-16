using System.Linq;
using AutoMapper;
using DatingApp.API.Dto;
using DatingApp.API.Models;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.PhotoUrl,
                       opt =>
                        {
                            opt.MapFrom(src =>
                                src.Photos
                                   .FirstOrDefault(p => p.IsMain)
                                   .Url);
                        })
                .ForMember(dest => dest.Age,
                       opt => opt.MapFrom(src => src.DateOfBirth.GetAge())
                      );

            CreateMap<User, DetailedUserDto>()
                .ForMember(dest => dest.PhotoUrl,
                       opt =>
                        {
                            opt.MapFrom(src =>
                                src.Photos
                                   .FirstOrDefault(p => p.IsMain)
                                   .Url);
                        })
                .ForMember(dest => dest.Age,
                       opt => opt.MapFrom(src => src.DateOfBirth.GetAge())
            );

            CreateMap<DetailedUserDto, User>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest=>dest.Photos,
                    opt=>opt.Ignore());
            
            CreateMap<Photo, PhotoDto>();
        }
    }
}