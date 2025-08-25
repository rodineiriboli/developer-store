using AutoMapper;
using DeveloperStore.Application.DTOs;
using DeveloperStore.Application.Mappings;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.Application.Tests.Mappings
{
    [Trait("Mapping", "Product")]
    public class ProductMappingProfileTests
    {
        private readonly IMapper _mapper;

        public ProductMappingProfileTests()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductMappingProfile>();
            });

            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void ShouldMapProductToProductDto()
        {
            // Arrange
            var rating = new Rating(4.5m, 100);
            var product = new Product(
                "Fjallraven Backpack",
                109.95m,
                "Your perfect pack for everyday use",
                "men's clothing",
                "https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg",
                rating
            );

            // Act
            var productDto = _mapper.Map<ProductDto>(product);

            // Assert
            Assert.Equal(product.Title, productDto.Title);
            Assert.Equal(product.Price, productDto.Price);
            Assert.Equal(product.Description, productDto.Description);
            Assert.Equal(product.Category, productDto.Category);
            Assert.Equal(product.Image, productDto.Image);
            Assert.Equal(product.Rating.Rate, productDto.Rating.Rate);
            Assert.Equal(product.Rating.Count, productDto.Rating.Count);
        }

        [Fact]
        public void ShouldMapRatingToRatingDto()
        {
            // Arrange
            var rating = new Rating(4.5m, 100);

            // Act
            var ratingDto = _mapper.Map<RatingDto>(rating);

            // Assert
            Assert.Equal(rating.Rate, ratingDto.Rate);
            Assert.Equal(rating.Count, ratingDto.Count);
        }
    }
}