using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Domain.Entities;

namespace DeveloperStore.Application.Mappings
{
    public class CartMappingProfile : Profile
    {
        public CartMappingProfile()
        {
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>();

            CreateMap<CreateCartDto, Cart>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());

            CreateMap<UpdateCartDto, Cart>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.Products, opt => opt.Ignore());
        }
    }
}