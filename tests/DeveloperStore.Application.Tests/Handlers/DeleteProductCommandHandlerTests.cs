using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace DeveloperStore.Application.Tests.Handlers
{
    [Trait("CommandHandler", "Product")]
    public class DeleteProductCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly DeleteProductCommandHandler _handler;

        public DeleteProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new DeleteProductCommandHandler(_productRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldDeleteProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new DeleteProductCommand { ProductId = productId };

            var product = CreateValidProduct();
            _productRepository.GetByIdAsync(productId).Returns(product);
            _productRepository.DeleteAsync(Arg.Any<Product>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            _mapper.Map<ProductDto>(Arg.Any<Product>()).Returns(new ProductDto { Id = productId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            await _productRepository.Received(1).DeleteAsync(Arg.Any<Product>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var command = new DeleteProductCommand { ProductId = Guid.NewGuid() };
            _productRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        private Product CreateValidProduct()
        {
            var rating = new Rating(4.5m, 100);
            return new Product(
                "Fjallraven Backpack",
                109.95m,
                "Your perfect pack for everyday use",
                "men's clothing",
                "https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg",
                rating
            );
        }
    }
}