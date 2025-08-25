using AutoMapper;
using DeveloperStore.Application.Commands;
using DeveloperStore.Application.Common.Interfaces;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using NSubstitute;
using Xunit;

namespace DeveloperStore.Application.Tests.Handlers
{
    public class UpdateProductCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UpdateProductCommandHandler _handler;

        public UpdateProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new UpdateProductCommandHandler(_productRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldUpdateProduct_WhenDataIsValid()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new UpdateProductCommand
            {
                ProductId = productId,
                ProductDto = new UpdateProductDto
                {
                    Title = "Updated Backpack",
                    Price = 129.95m,
                    Description = "Updated description",
                    Category = "women's clothing",
                    Image = "https://updated-image.jpg",
                    Rating = new RatingDto { Rate = 4.8m, Count = 200 }
                }
            };

            var existingProduct = CreateValidProduct();
            _productRepository.GetByIdAsync(productId).Returns(existingProduct);
            _productRepository.GetByTitleAsync(Arg.Any<string>()).Returns((Product)null);
            _productRepository.UpdateAsync(Arg.Any<Product>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            _mapper.Map<ProductDto>(Arg.Any<Product>()).Returns(new ProductDto { Id = productId });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            await _productRepository.Received(1).UpdateAsync(Arg.Any<Product>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var command = new UpdateProductCommand { ProductId = Guid.NewGuid() };
            _productRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenTitleExistsForOtherProduct()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new UpdateProductCommand
            {
                ProductId = productId,
                ProductDto = new UpdateProductDto { Title = "Existing Product" }
            };

            var existingProduct = CreateValidProduct();
            var otherProduct = CreateValidProduct();

            _productRepository.GetByIdAsync(productId).Returns(existingProduct);
            _productRepository.GetByTitleAsync(Arg.Any<string>()).Returns(otherProduct);

            // Act & Assert
            await Assert.ThrowsAsync<DomainException>(() =>
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