using DeveloperStore.Application.Queries;
using DeveloperStore.Application.Handlers;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Domain.ValueObjects;
using NSubstitute;
using Xunit;
using AutoMapper;
using DeveloperStore.Application.DTOs;

namespace DeveloperStore.Application.Tests.Handlers
{
    [Trait("CommandHandler", "Product")]
    public class GetProductByIdQueryHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly GetProductByIdQueryHandler _handler;

        public GetProductByIdQueryHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetProductByIdQueryHandler(_productRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var query = new GetProductByIdQuery { ProductId = productId };

            var product = CreateValidProduct();
            _productRepository.GetByIdAsync(productId).Returns(product);
            _mapper.Map<ProductDto>(product).Returns(new ProductDto { Id = productId });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var query = new GetProductByIdQuery { ProductId = Guid.NewGuid() };
            _productRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Product)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() =>
                _handler.Handle(query, CancellationToken.None));
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