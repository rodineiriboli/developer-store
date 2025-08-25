using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace DeveloperStore.Application.Tests.Handlers
{
    public class ClearCartCommandHandlerTests
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ClearCartCommandHandler _handler;

        public ClearCartCommandHandlerTests()
        {
            _cartRepository = Substitute.For<ICartRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new ClearCartCommandHandler(_cartRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldClearCart_WhenCartExists()
        {
            // Arrange
            var cartId = Guid.NewGuid();
            var command = new ClearCartCommand { CartId = cartId };

            var cart = CreateValidCart();
            cart.AddProduct(101, 2); // Add some products to clear

            _cartRepository.GetByIdAsync(cartId).Returns(cart);
            _cartRepository.UpdateAsync(Arg.Any<Cart>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            _mapper.Map<CartDto>(Arg.Any<Cart>()).Returns(new CartDto { Id = cartId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            await _cartRepository.Received(1).UpdateAsync(Arg.Any<Cart>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenCartNotFound()
        {
            // Arrange
            var command = new ClearCartCommand { CartId = Guid.NewGuid() };
            _cartRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Cart)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        private Cart CreateValidCart()
        {
            var cart = new Cart(1, DateTime.Now);
            return cart;
        }
    }
}