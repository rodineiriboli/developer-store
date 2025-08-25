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
    public class GetAllProductsQueryHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly GetAllProductsQueryHandler _handler;

        public GetAllProductsQueryHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetAllProductsQueryHandler(_productRepository, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldReturnProductsList_WhenProductsExist()
        {
            // Arrange
            var query = new GetAllProductsQuery { Page = 1, PageSize = 10 };

            var products = new List<Product> { CreateValidProduct(), CreateValidProduct() };
            _productRepository.GetAllAsync(1, 10).Returns(products);
            _mapper.Map<List<ProductDto>>(products).Returns(new List<ProductDto>
            {
                new ProductDto { Id = products[0].Id },
                new ProductDto { Id = products[1].Id }
            });

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var query = new GetAllProductsQuery { Page = 1, PageSize = 10 };

            _productRepository.GetAllAsync(1, 10).Returns(new List<Product>());
            _mapper.Map<List<ProductDto>>(Arg.Any<List<Product>>()).Returns(new List<ProductDto>());

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
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