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
    public class GetCartByIdQueryHandlerTests
    {
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly GetCartByIdQueryHandler _handler;

        public GetCartByIdQueryHandlerTests()
        {
            _cartRepository = Substitute.For<ICartRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetCartByIdQueryHandler(_cartRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnCart_WhenCartExists()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var query = new GetCartByIdQuery { CartId = cartId };

            var cart = CreateValidCart();
            _cartRepository.GetByIdAsync(cartId).Returns(cart);
            _mapper.Map<CartDto>(cart).Returns(new CartDto { Id = cartId });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cartId, result.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCartNotFound()
        {
            // Arrange
            var query = new GetCartByIdQuery { CartId = Guid.NewGuid() };
            _cartRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Cart)null);

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