using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.Interfaces;
using NSubstitute;
using Xunit;

namespace DeveloperStore.Application.Tests.Handlers
{
    public class CreateCartCommandHandlerTests
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CreateCartCommandHandler _handler;

        public CreateCartCommandHandlerTests()
        {
            _cartRepository = Substitute.For<ICartRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateCartCommandHandler(_cartRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldCreateCart_WhenDataIsValid()
        {
            // Arrange
            var command = new CreateCartCommand
            {
                CartDto = new CreateCartDto
                {
                    UserId = 1,
                    Date = DateTime.Now,
                    Products = new List<CartItemDto>
                    {
                        new CartItemDto { ProductId = 101, Quantity = 2 },
                        new CartItemDto { ProductId = 102, Quantity = 1 }
                    }
                }
            };

            _cartRepository.ExistsForUserAsync(Arg.Any<int>()).Returns(false);
            _cartRepository.AddAsync(Arg.Any<Cart>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            var expectedCart = CreateValidCart();
            _mapper.Map<CartDto>(Arg.Any<Cart>()).Returns(new CartDto { Id = expectedCart.Id });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            await _cartRepository.Received(1).AddAsync(Arg.Any<Cart>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenUserAlreadyHasCart()
        {
            // Arrange
            var command = new CreateCartCommand
            {
                CartDto = new CreateCartDto { UserId = 1 }
            };

            _cartRepository.ExistsForUserAsync(Arg.Any<int>()).Returns(true);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        private Cart CreateValidCart()
        {
            var cart = new Cart(1, DateTime.Now);
            return cart;
        }
    }
}