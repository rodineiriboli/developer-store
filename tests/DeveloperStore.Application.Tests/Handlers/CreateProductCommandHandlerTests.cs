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
    public class CreateProductCommandHandlerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mapper = Substitute.For<IMapper>();
            _handler = new CreateProductCommandHandler(_productRepository, _unitOfWork, _mapper);
        }

        [Fact]
        public async Task Handle_ShouldCreateProduct_WhenDataIsValid()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                ProductDto = new CreateProductDto
                {
                    Title = "Fjallraven Backpack",
                    Price = 109.95m,
                    Description = "Your perfect pack for everyday use",
                    Category = "men's clothing",
                    Image = "https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg",
                    Rating = new RatingDto { Rate = 4.5m, Count = 100 }
                }
            };

            _productRepository.ExistsByTitleAsync(Arg.Any<string>()).Returns(false);
            _productRepository.AddAsync(Arg.Any<Product>()).Returns(Task.CompletedTask);
            _unitOfWork.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);

            var expectedProduct = CreateValidProduct();
            _mapper.Map<ProductDto>(Arg.Any<Product>()).Returns(new ProductDto { Id = expectedProduct.Id });

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            await _productRepository.Received(1).AddAsync(Arg.Any<Product>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenTitleExists()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                ProductDto = new CreateProductDto { Title = "Existing Product" }
            };

            _productRepository.ExistsByTitleAsync(Arg.Any<string>()).Returns(true);

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