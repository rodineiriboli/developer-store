using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.ValueObjects;

namespace DeveloperStore.Application.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<Name, NameDto>();
            CreateMap<Address, AddressDto>();
            CreateMap<GeoLocation, GeoLocationDto>();

            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password));

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}