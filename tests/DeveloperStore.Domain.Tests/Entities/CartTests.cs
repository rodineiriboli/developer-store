using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Exceptions;
using Xunit;

namespace DeveloperStore.Domain.Tests.Entities
{
    public class CartTests
    {
        [Fact]
        public void Constructor_ShouldCreateCart_WithValidParameters()
        {
            // Arrange & Act
            var cart = new Cart(1, DateTime.Now);

            // Assert
            Assert.Equal(1, cart.UserId);
            Assert.NotNull(cart.Products);
            Assert.Empty(cart.Products);
        }

        [Fact]
        public void AddProduct_ShouldAddProduct_WhenQuantityIsValid()
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);

            // Act
            cart.AddProduct(101, 2);

            // Assert
            Assert.Single(cart.Products);
            Assert.Equal(101, cart.Products.First().ProductId);
            Assert.Equal(2, cart.Products.First().Quantity);
        }

        [Fact]
        public void AddProduct_ShouldUpdateQuantity_WhenProductAlreadyExists()
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);
            cart.AddProduct(101, 2);

            // Act
            cart.AddProduct(101, 3);

            // Assert
            Assert.Single(cart.Products);
            Assert.Equal(101, cart.Products.First().ProductId);
            Assert.Equal(5, cart.Products.First().Quantity);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void AddProduct_ShouldThrowException_WhenQuantityIsInvalid(int invalidQuantity)
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);

            // Act & Assert
            Assert.Throws<DomainException>(() => cart.AddProduct(101, invalidQuantity));
        }

        [Fact]
        public void UpdateProductQuantity_ShouldUpdateQuantity_WhenProductExists()
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);
            cart.AddProduct(101, 2);

            // Act
            cart.UpdateProductQuantity(101, 5);

            // Assert
            Assert.Equal(5, cart.Products.First().Quantity);
        }

        [Fact]
        public void UpdateProductQuantity_ShouldThrowException_WhenProductNotFound()
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);

            // Act & Assert
            Assert.Throws<DomainException>(() => cart.UpdateProductQuantity(999, 5));
        }

        [Fact]
        public void RemoveProduct_ShouldRemoveProduct_WhenProductExists()
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);
            cart.AddProduct(101, 2);
            cart.AddProduct(102, 1);

            // Act
            cart.RemoveProduct(101);

            // Assert
            Assert.Single(cart.Products);
            Assert.Equal(102, cart.Products.First().ProductId);
        }

        [Fact]
        public void RemoveProduct_ShouldDoNothing_WhenProductNotExists()
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);
            cart.AddProduct(101, 2);

            // Act
            cart.RemoveProduct(999); // Non-existent product

            // Assert
            Assert.Single(cart.Products); // Should still have the original product
        }

        [Fact]
        public void ClearCart_ShouldRemoveAllProducts()
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);
            cart.AddProduct(101, 2);
            cart.AddProduct(102, 1);
            cart.AddProduct(103, 3);

            // Act
            cart.ClearCart();

            // Assert
            Assert.Empty(cart.Products);
        }

        [Fact]
        public void ContainsProduct_ShouldReturnTrue_WhenProductExists()
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);
            cart.AddProduct(101, 2);

            // Act
            var contains = cart.ContainsProduct(101);

            // Assert
            Assert.True(contains);
        }

        [Fact]
        public void ContainsProduct_ShouldReturnFalse_WhenProductNotExists()
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);
            cart.AddProduct(101, 2);

            // Act
            var contains = cart.ContainsProduct(999);

            // Assert
            Assert.False(contains);
        }

        [Fact]
        public void GetProductQuantity_ShouldReturnQuantity_WhenProductExists()
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);
            cart.AddProduct(101, 2);

            // Act
            var quantity = cart.GetProductQuantity(101);

            // Assert
            Assert.Equal(2, quantity);
        }

        [Fact]
        public void GetProductQuantity_ShouldReturnZero_WhenProductNotExists()
        {
            // Arrange
            var cart = new Cart(1, DateTime.Now);

            // Act
            var quantity = cart.GetProductQuantity(999);

            // Assert
            Assert.Equal(0, quantity);
        }
    }
}