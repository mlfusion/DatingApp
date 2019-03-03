using System.Linq;
using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using System;

namespace DatingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt => {
                    opt.ResolveUsing(d => d.DateOfBirth.Value.CalculateAge());
                });

            CreateMap<User, UserForDetailedDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url);
                })
                .ForMember(dest => dest.Age, opt => {
                    opt.ResolveUsing(d => d.DateOfBirth.Value.CalculateAge());
                });
                
            CreateMap<Photo, PhotoForDetailDto>();

            CreateMap<UserForUpdateDto, User>()
                .ForMember(dest => dest.RoleId, opt => {
                    opt.ResolveUsing(src => src.RoleId);
                });

            CreateMap<Photo, PhotoForCreationDto>()
                .ForMember(dest => dest.Username, opt => {
                    opt.MapFrom(src => src.User.Username);
                }).ReverseMap();

            CreateMap<UserForRegisterDto, User>().ReverseMap();

            CreateMap<User, UserForReturnRegisterDto>()
                .ForMember(dest => dest.PhotoUrl, opt => {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url);
                });

            CreateMap<Role, RoleForDto>().ReverseMap();
        }
    }
}