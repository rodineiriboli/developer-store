using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Mappings;
using DeveloperStore.Domain.Entities;
using Xunit;

namespace DeveloperStore.Application.Tests.Mappings
{
    [Trait("Mapping", "Cart")]
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
            var cartId =  Guid.NewGuid();
            var productId1 = Guid.NewGuid();
            var productId2 = Guid.NewGuid();

            var cart = new Cart(cartId, DateTime.Now);
            cart.AddProduct(productId1, 2);
            cart.AddProduct(productId2, 1);

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
            var cartId = Guid.NewGuid();
            var cartItem = new CartItem(cartId, 2);

            // Act
            var cartItemDto = _mapper.Map<CartItemDto>(cartItem);

            // Assert
            Assert.Equal(cartItem.ProductId, cartItemDto.ProductId);
            Assert.Equal(cartItem.Quantity, cartItemDto.Quantity);
        }
    }
}