using DeveloperStore.Application.Queries;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using NSubstitute;
using Xunit;
using AutoMapper;
using DeveloperStore.Application.DTOs;

namespace DeveloperStore.Application.Tests.Handlers
{
    public class GetCartByUserIdQueryHandlerTests
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly GetCartByUserIdQueryHandler _handler;

        public GetCartByUserIdQueryHandlerTests()
        {
            _cartRepository = Substitute.For<ICartRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetCartByUserIdQueryHandler(_cartRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnCart_WhenCartExistsForUser()
        {
            // Arrange
            var userId = 1;
            var query = new GetCartByUserIdQuery { UserId = userId };

            var cart = CreateValidCart();
            _cartRepository.GetByUserIdAsync(userId).Returns(cart);
            _mapper.Map<CartDto>(cart).Returns(new CartDto { UserId = userId });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCartNotFoundForUser()
        {
            // Arrange
            var query = new GetCartByUserIdQuery { UserId = 999 };
            _cartRepository.GetByUserIdAsync(Arg.Any<int>()).Returns((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
        }

        private Cart CreateValidCart()
        {
            var cart = new Cart(1, DateTime.Now);
            cart.AddProduct(101, 2);
            return cart;
        }
    }
}