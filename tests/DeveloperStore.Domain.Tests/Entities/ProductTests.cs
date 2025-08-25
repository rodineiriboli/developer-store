using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using DeveloperStore.Domain.ValueObjects;
using Xunit;

namespace DeveloperStore.Domain.Tests.Entities
{
    public class ProductTests
    {
        private readonly Rating _validRating;

        public ProductTests()
        {
            _validRating = new Rating(4.5m, 100);
        }

        [Fact]
        public void Constructor_ShouldCreateProduct_WithValidParameters()
        {
            // Arrange & Act
            var product = new Product(
                "Fjallraven Backpack",
                109.95m,
                "Your perfect pack for everyday use",
                "men's clothing",
                "https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg",
                _validRating
            );

            // Assert
            Assert.Equal("Fjallraven Backpack", product.Title);
            Assert.Equal(109.95m, product.Price);
            Assert.Equal("Your perfect pack for everyday use", product.Description);
            Assert.Equal("men's clothing", product.Category);
            Assert.Equal("https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg", product.Image);
            Assert.Equal(_validRating, product.Rating);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_ShouldThrowException_WhenTitleIsInvalid(string invalidTitle)
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new Product(
                invalidTitle,
                109.95m,
                "Description",
                "Category",
                "image.jpg",
                _validRating
            ));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-10)]
        public void Constructor_ShouldThrowException_WhenPriceIsInvalid(decimal invalidPrice)
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new Product(
                "Test Product",
                invalidPrice,
                "Description",
                "Category",
                "image.jpg",
                _validRating
            ));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_ShouldThrowException_WhenDescriptionIsInvalid(string invalidDescription)
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new Product(
                "Test Product",
                109.95m,
                invalidDescription,
                "Category",
                "image.jpg",
                _validRating
            ));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Constructor_ShouldThrowException_WhenCategoryIsInvalid(string invalidCategory)
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new Product(
                "Test Product",
                109.95m,
                "Description",
                invalidCategory,
                "image.jpg",
                _validRating
            ));
        }

        [Fact]
        public void Constructor_ShouldThrowException_WhenRatingIsNull()
        {
            // Arrange & Act & Assert
            Assert.Throws<DomainException>(() => new Product(
                "Test Product",
                109.95m,
                "Description",
                "Category",
                "image.jpg",
                null
            ));
        }

        [Fact]
        public void Update_ShouldUpdateProductProperties_WithValidParameters()
        {
            // Arrange
            var product = CreateValidProduct();
            var newRating = new Rating(4.8m, 200);

            // Act
            product.Update(
                "Updated Backpack",
                129.95m,
                "Updated description",
                "women's clothing",
                "https://updated-image.jpg",
                newRating
            );

            // Assert
            Assert.Equal("Updated Backpack", product.Title);
            Assert.Equal(129.95m, product.Price);
            Assert.Equal("Updated description", product.Description);
            Assert.Equal("women's clothing", product.Category);
            Assert.Equal("https://updated-image.jpg", product.Image);
            Assert.Equal(newRating, product.Rating);
        }

        [Fact]
        public void UpdatePrice_ShouldUpdatePrice_WithValidValue()
        {
            // Arrange
            var product = CreateValidProduct();
            var newPrice = 89.95m;

            // Act
            product.UpdatePrice(newPrice);

            // Assert
            Assert.Equal(newPrice, product.Price);
        }

        [Fact]
        public void UpdatePrice_ShouldThrowException_WhenPriceIsInvalid()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act & Assert
            Assert.Throws<DomainException>(() => product.UpdatePrice(0));
        }

        [Fact]
        public void UpdateRating_ShouldUpdateRating_WithValidValue()
        {
            // Arrange
            var product = CreateValidProduct();
            var newRating = new Rating(4.8m, 200);

            // Act
            product.UpdateRating(newRating);

            // Assert
            Assert.Equal(newRating, product.Rating);
        }

        [Fact]
        public void Update_ShouldThrowException_WhenTitleIsInvalid()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act & Assert
            Assert.Throws<DomainException>(() => product.Update(
                "",
                109.95m,
                "Description",
                "Category",
                "image.jpg",
                _validRating
            ));
        }

        private Product CreateValidProduct()
        {
            return new Product(
                "Fjallraven Backpack",
                109.95m,
                "Your perfect pack for everyday use",
                "men's clothing",
                "https://fakestoreapi.com/img/81fPKd-2AYL._AC_SL1500_.jpg",
                _validRating
            );
        }
    }
}