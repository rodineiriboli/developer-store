using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Mappings;
using DeveloperStore.Domain.Entities;
using Xunit;

namespace DeveloperStore.Application.Tests.Mappings
{
    public class CartMappingProfileTests
    {
        private readonly IMapper _mapper;

        public CartMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CartMappingProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void ShouldMapCartToCartDto()
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);
            cart.AddProduct(101, 2);
            cart.AddProduct(102, 1);

            // Act
            var cartDto = _mapper.Map<CartDto>(cart);

            // Assert
            Assert.Equal(cart.UserId, cartDto.UserId);
            Assert.Equal(cart.Date, cartDto.Date);
            Assert.Equal(cart.Products.Count, cartDto.Products.Count);
        }

        [Fact]
        public void ShouldMapCartItemToCartItemDto()
        {
            // Arrange
            var cartItem = new CartItem(101, 2);

            // Act
            var cartItemDto = _mapper.Map<CartItemDto>(cartItem);

            // Assert
            Assert.Equal(cartItem.ProductId, cartItemDto.ProductId);
            Assert.Equal(cartItem.Quantity, cartItemDto.Quantity);
        }
    }
}